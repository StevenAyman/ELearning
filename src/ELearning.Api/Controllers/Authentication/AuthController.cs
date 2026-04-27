using System.Text.Json;
using System.Threading.Tasks;
using ELearning.Api.BackgroundJobs;
using ELearning.Api.DTOs.Users;
using ELearning.Api.Mappings.Assistants;
using ELearning.Api.Mappings.Instructors;
using ELearning.Api.Mappings.Users;
using ELearning.Api.Validators.Users;
using ELearning.Application.Exceptions;
using ELearning.Application.Instructors.CreateInstructor;
using ELearning.Application.Students.RegisterStudent;
using ELearning.Application.Users.GetUserWithIdentity;
using ELearning.Application.Users.UpdateUserProfile;
using ELearning.Domain.Users;
using ELearning.Infastructure.Services.KeycloakService;
using FluentValidation;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Api.Controllers.Authentication;
/// <summary>
/// Controller for all authentication and authorization endpoints
/// </summary>
/// <param name="logger">Logger used to log messages in structured format</param>
/// <param name="keycloakAdminApiService">Service that allow use deal with keycloak APIs</param>
/// <param name="sender">Used to executed handler related to command or query</param>
/// <param name="userProfileValidator">Validator for model</param>
/// <param name="keycloakUserDtoValidator">Validator for model</param>
[Route("api/keycloak")]
[ApiController]
[Produces("application/json")]
public class AuthController(ILogger<AuthController> logger, KeycloakAdminApiService keycloakAdminApiService, ISender sender,
    IValidator<UserProfileDto> userProfileValidator,
    IValidator<KeycloakUserDto> keycloakUserDtoValidator) : ControllerBase
{
    private readonly ILogger<AuthController> _logger = logger;
    private readonly KeycloakAdminApiService _keycloakAdminApiService = keycloakAdminApiService;
    private readonly ISender _sender = sender;
    private readonly IValidator<UserProfileDto> _userProfileValidator = userProfileValidator;
    private readonly IValidator<KeycloakUserDto> _keycloakUserDtoValidator = keycloakUserDtoValidator;

    /// <summary>
    /// Webhook endpoint called by keycloak in case of two events (register and email update)
    /// </summary>
    /// <param name="payload">Keycloak event details</param>
    /// <returns></returns>
    [HttpPost("webhooks")]
    [AllowAnonymous]
    public IActionResult ReceiveEvent([FromBody] KeycloakWebhookPayload payload)
    {
        if (payload.Type == "REGISTER")
        {
            var jobId = BackgroundJob.Enqueue<KeycloakRegisterUserJob>(job => job.ProcessAsync(payload.UserId));

            _logger.LogInformation("Job enqueued for registeration with ID {JobId}", jobId);
        }
        else if (payload.Type == "UPDATE_EMAIL")
        {
            var jobId = BackgroundJob.Enqueue<KeycloakUpdateUserEmailJob>(
                job => job.ProcessAsync(payload.Details["previous_email"], payload.Details["updated_email"]));

            _logger.LogInformation("Job enqueued for updating email with ID {JobId}", jobId);
        }

        return Ok();
    }
    /// <summary>
    /// Creates instructor in keycloak and record it in database
    /// </summary>
    /// <param name="userDto">The user required details for registeration</param>
    /// <returns></returns>
    /// <exception cref="Application.Exceptions.ValidationException">Exception thrown in case of invalid model param</exception>

    [HttpPost("instructor/create")]
    public async Task<IActionResult> CreateInstructor(KeycloakUserDto userDto)
    {
        var failures = await _keycloakUserDtoValidator.ValidateAsync(userDto);
        if (!failures.IsValid)
        {
            var errors = failures.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage)).ToList();
            if (errors.Any())
            {
                throw new Application.Exceptions.ValidationException(errors);
            }
        }
        var result = await _keycloakAdminApiService.RegisterUser(
            userDto.Username,
            userDto.FirstName,
            userDto.LastName,
            userDto.Email,
            userDto.City,
            userDto.BirthDate,
            userDto.Password,
            "instructor");

        if (result.IsFailure)
        {
            return Problem(
                result?.Error?.Message,
                title: result?.Error?.Code,
                statusCode: StatusCodes.Status400BadRequest);
        }


        var command = userDto.ToCreateInstructorCommand(result.Value);
        await _sender.Send(command);

        return Ok();
    }

    /// <summary>
    /// Creates assistant in keycloak and record it in database
    /// </summary>
    /// <param name="userDto">The user required details for registeration</param>
    /// <returns></returns>
    /// <exception cref="Application.Exceptions.ValidationException">Exception thrown in case of invalid model param</exception>

    [HttpPost("assistant/create")]
    public async Task<IActionResult> CreateAssistant(KeycloakUserDto userDto)
    {
        var failures = await _keycloakUserDtoValidator.ValidateAsync(userDto);
        if (!failures.IsValid)
        {
            var errors = failures.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage)).ToList();
            if (errors.Any())
            {
                throw new Application.Exceptions.ValidationException(errors);
            }
        }
        var result = await _keycloakAdminApiService.RegisterUser(
            userDto.Username,
            userDto.FirstName,
            userDto.LastName,
            userDto.Email,
            userDto.City,
            userDto.BirthDate,
            userDto.Password,
            "assistant");

        if (result.IsFailure)
        {
            return Problem(
                result?.Error?.Message,
                title: result?.Error?.Code,
                statusCode: StatusCodes.Status400BadRequest);
        }


        var command = userDto.ToCreateAssistantCommand(result.Value);
        await _sender.Send(command);

        return Ok();
    }

    /// <summary>
    /// Updates keycloak user password in keycloak
    /// </summary>
    /// <param name="userId">Identity user id</param>
    /// <returns></returns>
    [HttpPut("passwordchange/{userId}")]
    public async Task<IActionResult> ChangePassword(string userId)
    {
        var result = await _keycloakAdminApiService.ChangePassword(userId);

        if (result.IsFailure)
        {
            return Problem(
                detail:  result?.Error?.Message,
                title: result?.Error?.Code,
                statusCode: StatusCodes.Status400BadRequest);
        }

        return Ok();
    }

    /// <summary>
    /// Updates user email with verifcation in keycloak and updating database also
    /// </summary>
    /// <param name="userId">Identity user id used to change email</param>
    /// <returns></returns>

    [HttpPut("updateemail/{userId}")]
    public async Task<IActionResult> ChangeEmail(string userId)
    {
        var result = await _keycloakAdminApiService.ChangeEmailAsync(userId);

        if (result.IsFailure)
        {
            return Problem(
                detail: result?.Error?.Message,
                title: result?.Error?.Code,
                statusCode: StatusCodes.Status400BadRequest);
        }

        return Ok();
    }

    /// <summary>
    /// Updates user profile especially keycloak profile details
    /// </summary>
    /// <param name="userId">Identity user id</param>
    /// <param name="profile">Model that contains profile details to update</param>
    /// <returns></returns>
    /// <exception cref="Application.Exceptions.ValidationException">Exception thrown in case model is invalid</exception>

    [HttpPut("updateprofile/{userId}")]
    public async Task<IActionResult> UpdateProfile(string userId, UserProfileDto profile)
    {
        var failures = await _userProfileValidator.ValidateAsync(profile);
        if (!failures.IsValid)
        {
            var errors = failures.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage)).ToList();
            if (errors.Any())
            {
                throw new Application.Exceptions.ValidationException(errors);
            }
        }

        var keycloakUser = profile.ToKeycloakProfile();
        var result = await _keycloakAdminApiService.UpdateUserProfileAsync(userId, keycloakUser);

        if (result.IsFailure)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: result?.Error?.Code,
                detail: result?.Error?.Message);
        }

        var command = new UpdateUserProfileCommand(
            new FirstName(profile.FirstName),
            new LastName(profile.LastName),
            profile.City,
            profile.BirthDate,
            userId);

        var dbResult = await _sender.Send(command);

        if (dbResult.IsFailure)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: dbResult.Error?.Code,
                detail: dbResult.Error?.Message);
        }

        return NoContent();

    }

    /// <summary>
    /// Gets user details by identity id
    /// </summary>
    /// <param name="userId">Identity user id</param>
    /// <returns>user details</returns>

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<FullUserDto>> Get(string userId)
    {
        var user = await _keycloakAdminApiService.GetKeycloakUserById(userId);
        if (user is null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: UserErrors.UserNotExist.Code,
                detail:  UserErrors.UserNotExist.Message);
        }

        var query = new GetUserWithIdentityQuery(userId);
        var result = await _sender.Send(query);
        if (result.IsFailure)
        {
            return Problem(
                title: result?.Error?.Code,
                detail: result?.Error?.Message,
                statusCode: StatusCodes.Status404NotFound);
        }

        var userDto = result.Value.ToDto(user.Username);
        return Ok(userDto);
    }
}



