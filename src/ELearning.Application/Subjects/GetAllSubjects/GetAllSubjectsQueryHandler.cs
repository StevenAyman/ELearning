using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Subjects.GetSubject;
using ELearning.Domain.Shared;

namespace ELearning.Application.Subjects.GetAllSubjects;
internal sealed class GetAllSubjectsQueryHandler(
    ISubjectReadService subjectReadService) : IQueryHandler<GetAllSubjectsQuery, IEnumerable<SubjectDto>>
{
    private readonly ISubjectReadService _subjectReadService = subjectReadService;

    public async Task<Result<IEnumerable<SubjectDto>>> Handle(GetAllSubjectsQuery request, CancellationToken cancellationToken)
    {
        var subjects = await _subjectReadService.GetAllAsync(cancellationToken);

        return Result<IEnumerable<SubjectDto>>.Succuss(subjects);
    }
}
