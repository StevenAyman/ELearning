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

namespace ELearning.Application.Instructors.GetInstructor;
internal sealed class GetInstructorQueryHandler(
    IInstructorReadService instructorReadService,
    ISessionReadService sessionReadService) : IQueryHandler<GetInstructorQuery, InstructorWithSessionsDto>
{
    private readonly IInstructorReadService _instructorReadService = instructorReadService;
    private readonly ISessionReadService _sessionReadService = sessionReadService;

    public async Task<Result<InstructorWithSessionsDto>> Handle(GetInstructorQuery request, CancellationToken cancellationToken)
    {
        var instructor = await _instructorReadService.GetByIdAsync(request.Id, cancellationToken);

        if (instructor is null)
        {
            return Result<InstructorWithSessionsDto>.Failure(UserErrors.UserNotExist);
        }

        var sessions = await _sessionReadService.GetAllWithInstructorIdAsync(request.Id, cancellationToken);

        instructor.Sessions = sessions;

        return instructor;
    }
}
