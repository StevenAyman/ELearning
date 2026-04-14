using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Infastructure.Outbox;
public sealed class OutboxOptions
{
    public const string SectionName = "BackgroundJobs:Outbox";
    public string Schedule { get; init; }
    public int BatchSize { get; init; }
}
