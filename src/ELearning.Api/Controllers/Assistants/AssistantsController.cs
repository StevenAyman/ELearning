using ELearning.Api.Controllers.Authentication;
using ELearning.Api.DTOs.Assistants;
using ELearning.Api.DTOs.Shared;
using ELearning.Api.Services;
using ELearning.Application.Assistants.AssignNewInstructor;
using ELearning.Application.Assistants.DTOs;
using ELearning.Application.Assistants.GetAllAssistants;
using ELearning.Application.Assistants.GetAssistant;
using ELearning.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Api.Controllers.Assistants;
[Route("api/[controller]")]
[ApiController]
public class AssistantsController(ISender sender, LinkService linkService) : ControllerBase
{
    private readonly ISender _sender = sender;
    private readonly LinkService _linkService = linkService;

    /// <summary>
    /// Gets assistant by its (Unique Identifier)
    /// </summary>
    /// <param name="id">Assistant Identifier</param>
    /// <param name="accept">For Content Negotiation</param>
    /// <returns>Assistant with instructor details</returns>
    [ProducesResponseType<AssistantResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<AssistantResponse>> GetAssistant(string id, [FromHeader(Name = "Accept")] string accept)
    {
        var query = new GetAssistantQuery(id);
        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: StatusCodes.Status404NotFound);
        }

        var assistant = result.Value;
        if (accept == CustomMediaTypes.HateoasJson)
        {
            assistant.Links = GetLinks(id);
        }

        return Ok(assistant);
    }

    /// <summary>
    /// Gets all assistants which are related to instructor or all of them
    /// </summary>
    /// <param name="instructorId">instructor identifier as query param to filter assistants</param>
    /// <param name="accept">For Content Negotiation</param>
    /// <returns>All assistants</returns>
    [HttpGet]
    public async Task<ActionResult<AllDataDto<AssistantResponse>>> GetAll(string? instructorId, [FromHeader(Name = "Accept")] string accept)
    {
        var query = new GetAllAssistantsQuery(instructorId);
        var result = await _sender.Send(query);

        var assistants = result.Value;
        if (accept == CustomMediaTypes.HateoasJson)
        {
            foreach(var assistant in assistants)
            {
                assistant.Links = GetLinks(assistant.Id);
            }
        }

        var response = new AllDataDto<AssistantResponse>()
        {
            Data = assistants
        };

        return Ok(response);
    }

    /// <summary>
    /// Updates assistant's instructor that assigned to
    /// </summary>
    /// <param name="id">Assistant identifier</param>
    /// <param name="updateAssistantRequest">instructor identifier</param>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssistant(string id, UpdateAssistantRequest updateAssistantRequest)
    {
        var command = new AssignNewInstructorCommand(id, updateAssistantRequest.InstructorId);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: result.Error == UserErrors.UserNotExist ? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest);
        }

        return NoContent();
    }

    private LinkDto[] GetLinks(string id)
    {
        return [
            _linkService.Create(nameof(GetAssistant), "self", HttpMethods.Get, new {Id = id}),
            _linkService.Create(nameof(UpdateAssistant), "update", HttpMethods.Put, new {Id = id}),
            _linkService.Create(nameof(AuthController.CreateAssistant), "create", HttpMethods.Post, null, "Auth")
            ];
    }
}
