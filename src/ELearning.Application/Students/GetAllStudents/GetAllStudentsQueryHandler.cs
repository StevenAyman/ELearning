using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Common;
using ELearning.Application.Students.DTOs;
using ELearning.Domain.Shared;

namespace ELearning.Application.Students.GetAllStudents;
internal sealed class GetAllStudentsQueryHandler(
    IStudentReadService studentReadService) : IQueryHandler<GetAllStudentsQuery, PaginationDto<StudentDto>>
{
    private readonly IStudentReadService _studentReadService = studentReadService;

    public async Task<Result<PaginationDto<StudentDto>>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        var result = await _studentReadService.GetAllAsync(
            request.Search,
            request.Sort,
            request.ClassId,
            request.SubjectId,
            request.InstructorId,
            request.PageIndex,
            request.PageSize,
            cancellationToken);

        return result;
    }
}
