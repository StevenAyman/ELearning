using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Subjects.DTOs;
using ELearning.Domain.Shared;
using ELearning.Domain.Subjects;

namespace ELearning.Application.Subjects.GetSubject;
internal sealed class GetSubjectQueryHandler(
    ISubjectReadService subjectReadService,
    IInstructorReadService instructorReadService) : IQueryHandler<GetSubjectQuery, SubjectWithInstructorsDto>
{
    private readonly ISubjectReadService _subjectReadService = subjectReadService;
    private readonly IInstructorReadService _instructorReadService = instructorReadService;

    public async Task<Result<SubjectWithInstructorsDto>> Handle(GetSubjectQuery request, CancellationToken cancellationToken)
    {
        var subject = await _subjectReadService.GetByIdAsync(request.Id, cancellationToken);

        if (subject is null)
        {
            return Result<SubjectWithInstructorsDto>.Failure(SubjectErrors.NotFound);
        }

        var instructors = await _instructorReadService.GetWithSubjectIdAsync(request.Id, cancellationToken);
        var subjectWithInstructor = new SubjectWithInstructorsDto
        {
            Id = request.Id,
            Name = subject.Name,
            Instructors = instructors,
        };


        return subjectWithInstructor;
    }
}
