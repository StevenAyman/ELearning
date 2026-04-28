using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Application.Abstractions.Behaviors;
internal interface IInvalidatesCache
{
    IEnumerable<string> CacheKeys { get; }
}
