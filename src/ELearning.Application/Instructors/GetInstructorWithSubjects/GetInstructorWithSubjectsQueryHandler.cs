using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Instructors.DTOs;
using ELearning.Domain.Shared;
using ELearning.Domain.Users;

namespace ELearning.Application.Instructors.GetInstructorWithSubjects;
internal sealed class GetInstructorWithSubjectsQueryHandler(
    IInstructorReadService instructorReadService) : IQueryHandler<GetInstructorWithSubjectsQuery, InstructorSubjectsDto>
{
    private readonly IInstructorReadService _instructorReadService = instructorReadService;

    public async Task<Result<InstructorSubjectsDto>> Handle(GetInstructorWithSubjectsQuery request, CancellationToken cancellationToken)
    {
        var instructor = await _instructorReadService.GetInstructorWithSubjectsAsync(request.InstructorId, cancellationToken);

        if (instructor is null)
        {
            return Result<InstructorSubjectsDto>.Failure(UserErrors.UserNotExist);
        }

        return instructor;
    }
}
