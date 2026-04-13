using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Infastructure.Outbox;
public sealed class OutboxOptions
{
    public int InternalInSeconds { get; init; }
    public int BatchSize { get; init; }
}
