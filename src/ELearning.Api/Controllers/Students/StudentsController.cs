using ELearning.Api.DTOs.Shared;
using ELearning.Api.DTOs.Students;
using ELearning.Api.Services;
using ELearning.Api.Services.Sorting;
using ELearning.Application.Common;
using ELearning.Application.Students.DTOs;
using ELearning.Application.Students.GetAllStudents;
using ELearning.Application.Students.GetStudent;
using ELearning.Application.Students.UpdateStudent;
using ELearning.Domain.Classes;
using ELearning.Domain.Students;
using ELearning.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Api.Controllers.Students;
[Route("api/[controller]")]
[ApiController]
public class StudentsController(
    ISender sender,
    SortMappingProvider sortMappingProvider,
    LinkService linkService) : ControllerBase
{
    private readonly ISender _sender = sender;
    private readonly SortMappingProvider _sortMappingProvider = sortMappingProvider;
    private readonly LinkService _linkService = linkService;

    /// <summary>
    /// Gets student by its unique identifier
    /// </summary>
    /// <param name="id">student unique identifier</param>
    /// <returns>Student object</returns>
    [ProducesResponseType<StudentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> Get(string id)
    {
        var query = new GetStudentQuery(id);

        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets all available students based on query params
    /// </summary>
    /// <param name="getAllStudentsRequest">all applicable parameters to filter, search, sort, pagination</param>
    /// <returns>List of objects each one contains student info</returns>
    [ProducesResponseType<PaginationDto<StudentDto>>(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<PaginationDto<StudentDto>>> GetAll(GetAllStudentsRequest getAllStudentsRequest)
    {
        var mapping = _sortMappingProvider.GetMappings<StudentDto, Student>();
        var sort = SortMappingParser.ParseSort(getAllStudentsRequest.Sort!, mapping);
        var pageIndex = getAllStudentsRequest.PageIndex.HasValue ? getAllStudentsRequest.PageIndex.Value : 1;
        var pageSize = getAllStudentsRequest.PageSize.HasValue ? getAllStudentsRequest.PageSize.Value : 5;

        var query = new GetAllStudentsQuery(
            getAllStudentsRequest.ClassId,
            getAllStudentsRequest.Search,
            getAllStudentsRequest.SubjectId,
            getAllStudentsRequest.InstructorId,
            pageIndex,
            pageSize,
            sort);

        var result = await _sender.Send(query);

        var response = result.Value;
        if (getAllStudentsRequest.Accept == CustomMediaTypes.HateoasJson)
        {
            response.Links = CreateLinksForStudents(getAllStudentsRequest, response.HasPrevious, response.HasNext);
        }

        return Ok(response);
    }

    /// <summary>
    /// Update student class by class id and student id
    /// </summary>
    /// <param name="id">Student unique identifier</param>
    /// <param name="updateStudentRequest">Model should contains class identifier</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateStudentRequest updateStudentRequest)
    {
        var command = new UpdateStudentCommand(id, updateStudentRequest.ClassId);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: result.Error == UserErrors.UserNotExist || 
                            result.Error == ClassErrors.NotFound? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest);
        }

        return NoContent();
    }

    private List<LinkDto> CreateLinksForStudents(GetAllStudentsRequest getAllStudentsRequest, bool hasPrev, bool hasNext)
    {
        var list = new List<LinkDto>();
        list.Add(_linkService.Create(nameof(GetAll), "self", HttpMethods.Get, new
        {
            q = getAllStudentsRequest.Search,
            classId = getAllStudentsRequest.ClassId,
            subjectId = getAllStudentsRequest.SubjectId,
            instructorId = getAllStudentsRequest.InstructorId,
            sort = getAllStudentsRequest.Sort,
            pageIndex = getAllStudentsRequest.PageIndex,
            pageSize = getAllStudentsRequest.PageSize,
        }));
        
        if (hasPrev)
        {
            list.Add(_linkService.Create(nameof(GetAll), "prev-page", HttpMethods.Get, new
            {
                q = getAllStudentsRequest.Search,
                classId = getAllStudentsRequest.ClassId,
                subjectId = getAllStudentsRequest.SubjectId,
                instructorId = getAllStudentsRequest.InstructorId,
                sort = getAllStudentsRequest.Sort,
                pageIndex = getAllStudentsRequest.PageIndex - 1,
                pageSize = getAllStudentsRequest.PageSize,
            }));
        }

        if (hasNext)
        {
            list.Add(_linkService.Create(nameof(GetAll), "next-page", HttpMethods.Get, new
            {
                q = getAllStudentsRequest.Search,
                classId = getAllStudentsRequest.ClassId,
                subjectId = getAllStudentsRequest.SubjectId,
                instructorId = getAllStudentsRequest.InstructorId,
                sort = getAllStudentsRequest.Sort,
                pageIndex = getAllStudentsRequest.PageIndex + 1,
                pageSize = getAllStudentsRequest.PageSize,
            }));
        }

        return list;
    }
}


