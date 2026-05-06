using ELearning.Api.Controllers.Authentication;
using ELearning.Application.Students.RegisterStudent;
using ELearning.Domain.Users;
using ELearning.Infastructure.Services.KeycloakService;
using MediatR;

namespace ELearning.Api.BackgroundJobs;

public sealed class KeycloakRegisterUserJob(KeycloakAdminApiService keycloakAdminApiService, ILogger<KeycloakRegisterUserJob> logger, ISender sender)
{
    private readonly KeycloakAdminApiService _keycloakAdminApiService = keycloakAdminApiService;
    private readonly ILogger<KeycloakRegisterUserJob> _logger = logger;
    private readonly ISender _sender = sender;

    public async Task ProcessAsync(string userId)
    {
        var userInfo = await _keycloakAdminApiService.GetKeycloakUserById(userId);
        if (userInfo is null)
        {
            _logger.LogInformation("Invalid Register keycloak webhook call for user with id {UserId}", userId);
            return;
        }

        await _keycloakAdminApiService.AssignRoleForUser(userId, "student");

        if (userInfo.Attirbutes.Class is null)
        {
            return;
        }

        var userCommand = new RegisterStudentCommand(new FirstName(userInfo.FirstName),
            new LastName(userInfo.LastName),
            new Email(userInfo.Email),
            userInfo.Attirbutes.DateOfBirth,
            userInfo.Attirbutes.City,
            userInfo.Id,
            userInfo.Attirbutes.Class);
        var result = await _sender.Send(userCommand);

        if (result.IsFailure)
        {
            _logger.LogInformation("{ErrorMessage}", result.Error?.Message);
        }
        else
        {
            _logger.LogInformation("Student register successfully with id {StudentId}", userId);
        }
    }
}
