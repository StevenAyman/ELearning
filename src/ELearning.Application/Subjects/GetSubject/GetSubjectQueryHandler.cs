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
    ISubjectReadService subjectReadService) : IQueryHandler<GetSubjectQuery, SubjectResponse>
{
    private readonly ISubjectReadService _subjectReadService = subjectReadService;

    public async Task<Result<SubjectResponse>> Handle(GetSubjectQuery request, CancellationToken cancellationToken)
    {
        var subject = await _subjectReadService.GetByIdAsync(request.Id, cancellationToken);

        if (subject is null)
        {
            return Result<SubjectResponse>.Failure(SubjectErrors.NotFound);
        }

        var subjectWithInstructor = new SubjectResponse
        {
            Id = request.Id,
            Name = subject.Name
        };


        return subjectWithInstructor;
    }
}
