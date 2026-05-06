using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Cache;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Subjects.DTOs;

namespace ELearning.Application.Subjects.GetSubject;
public sealed record GetSubjectQuery(string Id) : ICachedQuery<SubjectResponse>
{
    public string CacheKey => $"subject:{Id}";

    public TimeSpan? Expiration => null;
}
