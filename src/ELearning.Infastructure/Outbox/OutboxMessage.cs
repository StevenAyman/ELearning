using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Infastructure.Outbox;
public sealed class OutboxMessage
{
    private OutboxMessage() { }
    public OutboxMessage(
        Guid id,
        DateTime occurredOn,
        string type,
        string content)
    {
        Id = id;
        Type = type;
        Content = content;
        OccurredOnUtc = occurredOn;
    }
    public Guid Id { get; init; }
    public string Type { get; init; }
    public string Content { get; init; }
    public DateTime OccurredOnUtc { get; init; }
    public DateTime? ProcessedOnUtc { get; init; }
    public string? Error { get; init; }
}
