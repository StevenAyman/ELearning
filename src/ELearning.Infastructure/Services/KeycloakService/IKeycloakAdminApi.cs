using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Infastructure.Services.KeycloakService.DTOs;
using Microsoft.AspNetCore.Mvc;
using Refit;
using static ELearning.Infastructure.Services.KeycloakService.KeycloakAdminApiService;

namespace ELearning.Infastructure.Services.KeycloakService;
public interface IKeycloakAdminApi
{
    [Get("/admin/realms/{realm}/users/{userId}")]
    Task<ApiResponse<KeycloakUserDto>> GetUserInfo(string realm,string userId);

    [Get("/admin/realms/{realm}/roles/{roleName}")]
    Task<KeycloakRoleDto> GetRoleByNameAsync(string realm, string roleName);

    [Post("/admin/realms/{realm}/users")]
    Task<IApiResponse> CreateUserAsync(string realm, [Body] KeycloakUserRegisterationDto user);

    [Post("/admin/realms/{realm}/users/{userId}/role-mappings/realm")]
    Task<IApiResponse> AssignRoleAsync(string realm, string userId, [Body] List<KeycloakRoleDto> roles);


    [Delete("/admin/realms/{realm}/users/{userId}/role-mappings/realm")]
    Task<IApiResponse> DeleteRoleAsync(string realm, string userId, [Body] List<KeycloakRoleDto> roles);

    [Put("/admin/realms/{realm}/users/{userId}/execute-actions-email")]
    Task<IApiResponse> ChangePasswordAsync(
        string realm, 
        string userId, 
        [Body] List<string> body);

    [Put("/admin/realms/{realm}/users/{userId}/execute-actions-email")]
    Task<IApiResponse> ChangeEmailAsync(string realm, string userId, [Body] List<string> body,
        [Query][AliasAs("redirect_uri")] string redirectUri,
        [Query][AliasAs("client_id")] string clientId);

    [Put("/admin/realms/{realm}/users/{userId}")]
    Task<IApiResponse> UpdateProfileAsync(string realm, string userId, [Body] KeycloakUserProfileDto keycloakUserProfileDto);

}
