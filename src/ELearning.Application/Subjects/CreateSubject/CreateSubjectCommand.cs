using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Behaviors;
using ELearning.Application.Abstractions.Messaging;

namespace ELearning.Application.Subjects.CreateSubject;
public sealed record CreateSubjectCommand(string Name) : ICommand<string>, IInvalidatesCache
{
    public IEnumerable<string> CacheKeys => ["subjects"];
}
