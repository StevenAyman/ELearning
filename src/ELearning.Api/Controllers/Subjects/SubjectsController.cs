using ELearning.Api.DTOs.Shared;
using ELearning.Api.DTOs.Subjects;
using ELearning.Api.Services;
using ELearning.Application.Subjects.CreateSubject;
using ELearning.Application.Subjects.DeleteSubject;
using ELearning.Application.Subjects.DTOs;
using ELearning.Application.Subjects.GetAllSubjects;
using ELearning.Application.Subjects.GetSubject;
using ELearning.Application.Subjects.UpdateSubject;
using ELearning.Domain.Subjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Api.Controllers.Subjects;
[Route("api/[controller]")]
[ApiController]
[ProducesErrorResponseType(typeof(ProblemDetails))]
public class SubjectsController(ISender sender, LinkService linkService) : ControllerBase
{
    private readonly ISender _sender = sender;
    private readonly LinkService _linkService = linkService;

    /// <summary>
    /// Gets specific subject with its instructors that teach it by (Subject Id)
    /// </summary>
    /// <param name="id">Subject Unique Identifier</param>
    /// <param name="accept">Accepted media type for content negotiation</param>
    /// <returns>Subject model contains subject details with all instructors related to that subject</returns>
    [ProducesResponseType<SubjectResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<SubjectResponse>> Get(string id, [FromHeader(Name = "Accept")] string accept)
    {
        var query = new GetSubjectQuery(id);
        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: result.Error?.Code,
                detail: result.Error?.Message);
        }
        
        result.Value.Links = accept == CustomMediaTypes.HateoasJson ? GetLinks(id) : null;

        return Ok(result.Value);
    }

    /// <summary>
    /// Create new not exist subject
    /// </summary>
    /// <param name="createSubjectDto">Model contains the name of the new subject</param>
    /// <param name="accept">Accepted media type for content negotiation</param>
    /// <returns></returns>
    [ProducesResponseType<SubjectDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<SubjectDto>> Create(CreateSubjectDto createSubjectDto, [FromHeader(Name = "Accept")] string accept)
    {
        var command = new CreateSubjectCommand(createSubjectDto.SubjectName);
        var result = await _sender.Send(command);
        if (result.IsFailure)
        {
            return Problem(
                statusCode: StatusCodes.Status409Conflict,
                detail: result.Error?.Message,
                title: result.Error?.Code);
        }

        var subjectResponse = new SubjectDto
        {
            Id = result.Value,
            Name = createSubjectDto.SubjectName,
            Links = accept == CustomMediaTypes.HateoasJson? GetLinks(result.Value) : null
        };

        return CreatedAtAction(nameof(Get), new { Id = subjectResponse.Id }, subjectResponse);
    }

    /// <summary>
    /// Gets all subjects that are exist in our system
    /// </summary>
    /// <returns>List of each subject with its details</returns>
    [ProducesResponseType<AllDataDto<SubjectDto>>(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<AllDataDto<SubjectDto>>> GetAll([FromHeader(Name = "Accept")] string accept)
    {
        var query = new GetAllSubjectsQuery();
        var result = await _sender.Send(query);
        var subjects = result.Value;

        if (accept == CustomMediaTypes.HateoasJson)
        {
            foreach (var subject in subjects)
            {
                subject.Links = GetLinks(subject.Id);
            }
        }

        var response = new AllDataDto<SubjectDto>
        {
            Data = result.Value
        };

        return Ok(response);
    }

    /// <summary>
    /// Updates subject name without one not exist
    /// </summary>
    /// <param name="id">The subject unique identifier that identify subject to update</param>
    /// <param name="createSubjectDto"></param>
    /// <returns>No content for valid update request</returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, CreateSubjectDto createSubjectDto)
    {
        var command = new UpdateSubjectCommand(id, createSubjectDto.SubjectName);
        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            var problem = Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code);
            
            if (result.Error == SubjectErrors.NotFound)
            {
                problem.StatusCode = StatusCodes.Status404NotFound;
            }
            else if (result.Error == SubjectErrors.Duplicate)
            {
                problem.StatusCode = StatusCodes.Status409Conflict;
            }

            return problem;
        }

        return NoContent();
    }

    /// <summary>
    /// Delete subject by its id
    /// </summary>
    /// <param name="id">The subject unique identifier that specify which subject to delete</param>
    /// <returns>No content for valid delete request</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var command = new DeleteSubjectCommand(id);
        var result = await _sender.Send(command);

        if (result.IsFailure && result.Error == SubjectErrors.NotFound)
        {
            return Problem(
                result.Error?.Message,
                result.Error?.Code,
                statusCode: StatusCodes.Status404NotFound);
        }
        else if (result.IsFailure)
        {
            return Problem(
                result.Error?.Message,
                result.Error?.Code,
                statusCode: StatusCodes.Status400BadRequest);
        }

        return NoContent();
    }

    private LinkDto[] GetLinks(string id)
    {
        return [
            _linkService.Create(nameof(Get), "self", HttpMethods.Get, new {Id = id}),
            _linkService.Create(nameof(Update), "update", HttpMethods.Put, new {Id = id}),
            _linkService.Create(nameof(Delete), "delete", HttpMethods.Delete, new {Id = id}),
            _linkService.Create(nameof(Create), "create", HttpMethods.Post)
            ];
    }

}
