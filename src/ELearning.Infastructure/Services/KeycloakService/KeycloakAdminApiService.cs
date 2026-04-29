using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;
using ELearning.Domain.Users;
using ELearning.Infastructure.Services.KeycloakService.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ELearning.Infastructure.Services.KeycloakService;
public sealed class KeycloakAdminApiService(IKeycloakAdminApi keycloakAdminApi, IOptions<KeycloakOptions> options, ILogger<KeycloakAdminApiService> logger)
{
    private readonly IKeycloakAdminApi _keycloakAdminApi = keycloakAdminApi;
    private readonly KeycloakOptions _options = options.Value;
    private readonly ILogger<KeycloakAdminApiService> _logger = logger;
    public async Task<KeycloakUserDto?> GetKeycloakUserById(string userId)
    {
        var userInfo = await _keycloakAdminApi.GetUserInfo(_options.Realm, userId);
        if (!userInfo.IsSuccessful)
        {
            return null;
        }

        if (userInfo.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        return userInfo.Content;
    }

    public async Task<Result<string>> RegisterUser(
        string username, 
        string firstName, 
        string lastName, 
        string email,
        string city,
        string birthdate,
        string password,
        string roleName)
    {
        // 1. Create new user
        _logger.LogInformation("Start to create new {Role}", roleName);

        var credential = new KeycloakCredentialsDto(password);
        var user = new KeycloakUserRegisterationDto(
            username,
            email,
            firstName,
            lastName);
        user.Attributes.TryAdd("city", [city]);
        user.Attributes.TryAdd("date_of_birth", [birthdate]);
        user.Credentials.Add(credential);

        // 2. Send request to create at keycloak
        var res = await _keycloakAdminApi.CreateUserAsync(_options.Realm, user);
        if (!res.IsSuccessful)
        {
            _logger.LogInformation(res.Error, "Error while trying to create user in keycloak with status code {StatusCode}", res.StatusCode);
            _logger.LogInformation("{ErrorMessage}", res.Error.Content);
            return Result<string>.Failure(UserErrors.InvalidRegister);
        }

        var requiredRole = await GetKeycloakRoleByName(roleName);
        if (requiredRole is null)
        {
            _logger.LogInformation("Role {RoleName} doesn't exist while trying to assign it to user", roleName);
            return Result<string>.Failure(UserErrors.InvalidRole);
        }

        var userLocationHeader = res?.Headers?.Location?.ToString();
        var userId = userLocationHeader?.Split("/")[^1];
        if (userId is null)
        {
            return Result<string>.Failure(UserErrors.UserNotExist);
        }
        // 3. Assign the required role to the created user
        var response = await _keycloakAdminApi.AssignRoleAsync(_options.Realm, userId, [requiredRole]);

        if (!response.IsSuccessful)
        {
            _logger.LogInformation(response.Error, "Can't Assign user with Id {UserId} to Role {RoleName}", userId, roleName);
            return Result<string>.Failure(UserErrors.InvalidRole);
        }

        _logger.LogInformation("Completed user creation with id {UserId} and Role {RoleName} successfully.", userId, requiredRole.Name);
        return Result<string>.Succuss(userId);
    }

    public async Task<KeycloakRoleDto?> GetKeycloakRoleByName(string name)
    {
        var response = await _keycloakAdminApi.GetRoleByNameAsync(_options.Realm, name);

        return response;
    }

    public async Task DeleteUserRoleAsync(string userId, string roleName)
    {
        var roleToRemove = await GetKeycloakRoleByName(roleName);
        if (roleToRemove is null)
        {
            return;
        }

        var res = await _keycloakAdminApi.DeleteRoleAsync(_options.Realm, userId, [roleToRemove]);

        if (!res.IsSuccessful)
        {
            _logger.LogInformation("Failed to remove role {RoleName} for student with id {Id}", roleName, userId);
            return;
        }

        _logger.LogInformation("Role {RoleName} is removed successfully from user with id {Id}", roleName, userId);
    }

    public async Task AssignRoleForUser(string userId, string roleName)
    {
        var roleToRemove = await GetKeycloakRoleByName(roleName);
        if (roleToRemove is null)
        {
            return;
        }

        var res = await _keycloakAdminApi.AssignRoleAsync(_options.Realm, userId, [roleToRemove]);

        if (!res.IsSuccessful)
        {
            _logger.LogInformation("Failed to assign role {RoleName} for student with id {Id}", roleName, userId);
            return;
        }

        _logger.LogInformation("Role {RoleName} is assigned successfully to user with id {Id}", roleName, userId);
    }

    public async Task<Result> ChangePassword(string userId)
    {
        var response = await _keycloakAdminApi.ChangePasswordAsync(_options.Realm, userId, ["UPDATE_PASSWORD"]);

        if (!response.IsSuccessful)
        {
            _logger.LogInformation(response.Error, "Something went wrong while trying to change password");
            return Result.Failure(new Error("Authentication.PasswordChange", "Something went wrong while trying to change password"));
        }

        _logger.LogInformation("Change password request sent successfully with status code {StatusCode}", response.StatusCode);
        return Result.Success();
    }

    public async Task<Result> ChangeEmailAsync(string userId)
    {
        var res = await _keycloakAdminApi.ChangeEmailAsync(_options.Realm, userId, ["VERIFY_EMAIL", "UPDATE_EMAIL"], _options.RedirectUri, _options.ApplicationClientId);

        if (!res.IsSuccessful)
        {
            _logger.LogError(res.Error, "Something went wrong while trying to process email update request, {StatusCode}", res.StatusCode);

            return Result.Failure(new Error("Authentication.EmailChange", "Something went wrong while trying to change email"));
        }

        _logger.LogInformation("Changing email request is processed successfully {StatusCode}", res.StatusCode);
        return Result.Success();
    }

    public async Task<Result> UpdateUserProfileAsync(string userId, KeycloakUserProfileDto keycloakUserProfileDto)
    {
        var userInfo = await _keycloakAdminApi.GetUserInfo(_options.Realm, userId);
        if (!userInfo.IsSuccessful)
        {
            return Result.Failure(UserErrors.UserNotExist);
        }
        keycloakUserProfileDto.Email = userInfo.Content.Email;
        var res = await _keycloakAdminApi.UpdateProfileAsync(_options.Realm, userId, keycloakUserProfileDto);

        if (!res.IsSuccessful)
        {
            _logger.LogError(res.Error, "An error has been occurred while trying to update user profile");
            return Result.Failure(new Error("User.ProfileUpdate", res?.Error?.Content ?? ""));
        }

        return Result.Success();
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var result = await _keycloakAdminApi.DeleteUserAsync(_options.Realm, userId);

        if (!result.IsSuccessful)
        {
            _logger.LogInformation("Removing user from keycloak not succeeded with error {Error}", result.Error?.Content);
            return Result.Failure(new Error(result.StatusCode.ToString(), $"{result.Error?.Content}"));
        }

        return Result.Success();
    }
}
