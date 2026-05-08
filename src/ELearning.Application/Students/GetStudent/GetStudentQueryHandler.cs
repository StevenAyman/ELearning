using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Students.DTOs;
using ELearning.Domain.Shared;
using ELearning.Domain.Users;

namespace ELearning.Application.Students.GetStudent;
internal sealed class GetStudentQueryHandler(
    IStudentReadService studentReadService) : IQueryHandler<GetStudentQuery, StudentDto>
{
    private readonly IStudentReadService _studentReadService = studentReadService;

    public async Task<Result<StudentDto>> Handle(GetStudentQuery request, CancellationToken cancellationToken)
    {
        var student = await _studentReadService.GetStudentByIdAsync(request.Id, cancellationToken);

        if (student is null)
        {
            return Result<StudentDto>.Failure(UserErrors.UserNotExist);
        }

        return student;
    }
}
