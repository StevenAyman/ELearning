using ELearning.Api.Controllers.Authentication;
using ELearning.Api.DTOs.Classes;
using ELearning.Api.DTOs.Shared;
using ELearning.Api.Services;
using ELearning.Application.Classes.AddSubjectToClass;
using ELearning.Application.Classes.CreateClass;
using ELearning.Application.Classes.DeleteClass;
using ELearning.Application.Classes.DTOs;
using ELearning.Application.Classes.GetAllClasses;
using ELearning.Application.Classes.GetClass;
using ELearning.Application.Classes.RemoveSubjectFromClass;
using ELearning.Application.Classes.UpdateClass;
using ELearning.Domain.Classes;
using ELearning.Domain.Shared;
using ELearning.Domain.Subjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Api.Controllers.Classes;
[Route("api/[controller]")]
[ApiController]
public class ClassesController(
    ISender sender, LinkService linkService) : ControllerBase
{
    private readonly ISender _sender = sender;
    private readonly LinkService _linkService = linkService;

    /// <summary>
    /// Gets All available classes
    /// </summary>
    /// <param name="accept">Responsible for content negotiation</param>
    /// <returns>All classes with details</returns>
    [ProducesResponseType<AllDataDto<ClassDto>>(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<AllDataDto<ClassDto>>> GetAll([FromHeader(Name = "Accept")] string accept)
    {
        var query = new GetAllClassesQuery();
        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: StatusCodes.Status400BadRequest);
        }

        var response = new AllDataDto<ClassDto>()
        {
            Data = result.Value,
        };

        if (accept == CustomMediaTypes.HateoasJson)
        {
            foreach(var learningClass in response.Data)
            {
                learningClass.Links = GetLinks(learningClass.Id);
            }
        }

        return Ok(response);
    }

    /// <summary>
    /// Gets specific class with its identifier and all subjects assigned to
    /// </summary>
    /// <param name="id">Class Unique Identifier</param>
    /// <param name="accept">Responsible for content negotiation</param>
    /// <returns>Class details with subjects</returns>
    [ProducesResponseType<ClassWithSubjectsDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ClassWithSubjectsDto>> GetClass(string id, [FromHeader(Name = "Accept")] string accept)
    {
        var query = new GetClassQuery(id);
        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: StatusCodes.Status404NotFound);
        }

        var response = result.Value;
        response.Links = accept == CustomMediaTypes.HateoasJson ? GetLinks(id) : null;

        return result.Value;
    }

    /// <summary>
    /// Creates new class with name currently not exist
    /// </summary>
    /// <param name="createClassRequest">Model contains class name that should be created</param>
    /// <param name="accept">Responsible for content negotiation</param>
    /// <returns>Created class</returns>
    [ProducesResponseType<ClassDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<ClassDto>> CreateClass(CreateClassRequest createClassRequest, [FromHeader(Name = "Accept")] string accept)
    {
        var command = new CreateClassCommand(createClassRequest.Class);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: result.Error == ClassErrors.IsExist? StatusCodes.Status409Conflict : StatusCodes.Status400BadRequest);
        }
        var response = new ClassDto
        {
            Id = result.Value,
            Class = createClassRequest.Class
        };

        if (accept  == CustomMediaTypes.HateoasJson)
        {
            response.Links = GetLinks(response.Id);
        }

        return CreatedAtAction(nameof(GetClass), new { response.Id }, response);
    }

    /// <summary>
    /// Updates class name
    /// </summary>
    /// <param name="id">Class Unique Identifier</param>
    /// <param name="createClassRequest">Object contains new valid name</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClass(string id, CreateClassRequest createClassRequest)
    {
        var command = new UpdateClassCommand(id, createClassRequest.Class);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            var problem = Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code);

            if (result.Error == ClassErrors.IsExist)
            {
                problem.StatusCode = StatusCodes.Status409Conflict;
            }

            if (result.Error == ClassErrors.NotFound)
            {
                problem.StatusCode = StatusCodes.Status404NotFound;
            }

            if (result.Error == Errors.DatabaseError)
            {
                problem.StatusCode = StatusCodes.Status400BadRequest;
            }

            return problem;
        }

        return NoContent();
    }

    /// <summary>
    /// Removes a class with its identifier
    /// </summary>
    /// <param name="id">Class Unique Identifier</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClass(string id)
    {
        var command = new DeleteClassCommand(id);

        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: result.Error == ClassErrors.NotFound? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest);
        }
        return NoContent();
    }

    /// <summary>
    /// Adds specific subject to specific class using their ids
    /// </summary>
    /// <param name="id">Class unique identifier</param>
    /// <param name="subjectId">Subject unique identifier</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("{id}/subjects/{subjectId}")]
    public async Task<IActionResult> AddSubjectToClass(string id, string subjectId)
    {
        var command = new AddSubjectToClassCommand(id, subjectId);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            var problem = Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code);

            if (result.Error ==  ClassErrors.NotFound || result.Error == SubjectErrors.NotFound)
            {
                problem.StatusCode = StatusCodes.Status404NotFound;
            } 
            else
            {
                problem.StatusCode = StatusCodes.Status400BadRequest;
            }

            return problem;
        }
        return NoContent();
    }

    /// <summary>
    /// Remove specific subject from specific class using their ids
    /// </summary>
    /// <param name="id">Class unique identifier</param>
    /// <param name="subjectId">Subject unique identifiers</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("{id}/subjects/{subjectId}")]
    public async Task<IActionResult> RemoveSubjectFromClass(string id, string subjectId)
    {
        var command = new RemoveSubjectFromClassCommand(id, subjectId);
        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: result.Error == ClassErrors.SubjectNotInClass? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest);
        }

        return NoContent();
    }

    private LinkDto[] GetLinks(string id)
    {
        return [
            _linkService.Create(nameof(GetClass), "self", HttpMethods.Get, new {Id = id}),
            _linkService.Create(nameof(UpdateClass), "update", HttpMethods.Put, new {Id = id}),
            _linkService.Create(nameof(CreateClass), "create", HttpMethods.Post),
            _linkService.Create(nameof(DeleteClass), "delete", HttpMethods.Delete, new {Id = id})
            ];
    }
}
