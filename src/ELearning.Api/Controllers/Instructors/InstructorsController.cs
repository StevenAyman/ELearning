using ELearning.Api.Controllers.Authentication;
using ELearning.Api.DTOs.Instructors;
using ELearning.Api.DTOs.Shared;
using ELearning.Api.Services;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Instructors.DTOs;
using ELearning.Application.Instructors.GetAllInstructors;
using ELearning.Application.Instructors.GetInstructor;
using ELearning.Application.Instructors.UpdateInstructor;
using ELearning.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ELearning.Api.Controllers.Instructors;
[Route("api/[controller]")]
[ApiController]
public class InstructorsController(ISender sender, LinkService linkService) : ControllerBase
{
    private readonly ISender _sender = sender;
    private readonly LinkService _linkService = linkService;

    /// <summary>
    /// Gets instructors by its unique id
    /// </summary>
    /// <param name="id">Instructor Identifier</param>
    /// <param name="accept">header paramter for content negotiation</param>
    /// <returns>Instructor with all sessions uploaded</returns>
    [ProducesResponseType<InstructorWithSessionsDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<InstructorWithSessionsDto>> GetInstructor(string id, [FromHeader(Name = "accept")] string accept)
    {
        var query = new GetInstructorQuery(id);

        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return Problem(
                result.Error?.Message,
                result.Error?.Code,
                statusCode: StatusCodes.Status404NotFound);
        }

        var ins = result.Value;
        ins.Links = accept == CustomMediaTypes.HateoasJson ? GetLinks(id) : null;

        return Ok(result.Value);
    }

    /// <summary>
    /// Updates instructor bio by his Identifier
    /// </summary>
    /// <param name="id">Instructor uniquer identifier</param>
    /// <param name="updateInstructorRequest">Model that represents the new value for bio</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInstructor(string id, UpdateInstructorRequest updateInstructorRequest)
    {
        var command = new UpdateInstructorCommand(id, updateInstructorRequest.Bio);
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

    /// <summary>
    /// Gets all existing instructors
    /// </summary>
    /// <returns>All instructors</returns>
    [ProducesResponseType<AllDataDto<InstructorDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<ActionResult<AllDataDto<InstructorDto>>> GetAllInstructors([FromHeader(Name = "Accept")] string accept)
    {
        var query = new GetAllInstructorsQuery();
        var instructors = await _sender.Send(query);

        if (instructors.IsFailure)
        {
            return Problem(
                detail: instructors.Error?.Message,
                title: instructors.Error?.Code,
                statusCode: StatusCodes.Status400BadRequest);
        }

        var ins = instructors.Value;

        if (accept == CustomMediaTypes.HateoasJson)
        {
            foreach (var instructor in ins)
            {
                instructor.Links = GetLinks(instructor.Id);
            }
        }
        

        var response = new AllDataDto<InstructorDto>()
        {
            Data = ins,
        };

        return Ok(response);
    }

    private LinkDto[] GetLinks(string id)
    {
        return [
            _linkService.Create(nameof(GetInstructor), "self", HttpMethods.Get, new {Id = id}),
            _linkService.Create(nameof(UpdateInstructor), "update", HttpMethods.Put, new {Id = id}),
            _linkService.Create(nameof(AuthController.CreateInstructor), "create", HttpMethods.Post, null, "Auth")
            ];
    }
}
