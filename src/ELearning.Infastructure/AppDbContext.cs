using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ELearning.Domain.Shared;
using ELearning.Infastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ELearning.Infastructure;
public sealed class AppDbContext : DbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddDomainEventsAsOutboxMessages();

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    public void AddDomainEventsAsOutboxMessages()
    {
        var outboxMessages = ChangeTracker.Entries<BaseEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(e =>
            {
                var domainEvents = e.DomainEvents;
                e.ClearDomainEvents();

                return domainEvents;
            })
            .Select(d => new OutboxMessage(
                Guid.CreateVersion7(),
                DateTime.UtcNow,
                d.GetType().Name,
                JsonConvert.SerializeObject(d, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                })
            )).ToList();

        AddRange(outboxMessages);
    }

}
