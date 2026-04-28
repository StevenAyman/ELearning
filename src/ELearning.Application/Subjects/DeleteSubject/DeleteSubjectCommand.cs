using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Behaviors;
using ELearning.Application.Abstractions.Messaging;

namespace ELearning.Application.Subjects.DeleteSubject;
public sealed record DeleteSubjectCommand(string Id) : ICommand, IInvalidatesCache
{
    public IEnumerable<string> CacheKeys => ["subjects", $"subject:{Id}"];
}
