using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Domain.Shared;

public abstract class BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    protected BaseEntity() { }
    protected BaseEntity(string id)
    {
        Id = id;
    }
    public string Id { get; private set; }
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;
    public void ClearDomainEvents() => _domainEvents.Clear();
    public void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
