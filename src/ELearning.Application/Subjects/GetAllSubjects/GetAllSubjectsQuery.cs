using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Cache;
using ELearning.Application.Subjects.GetSubject;

namespace ELearning.Application.Subjects.GetAllSubjects;
public sealed record GetAllSubjectsQuery() : ICachedQuery<IEnumerable<SubjectDto>>
{
    public string CacheKey => $"subjects";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
}
