using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ELearning.Application.Abstractions.Clock;
using ELearning.Application.Abstractions.Data;
using ELearning.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ELearning.Infastructure.Outbox;
public sealed class ProcessOutboxMessageJob(
    IDbConnectionFactory dbConnectionFactory, 
    ILogger<ProcessOutboxMessageJob> logger,
    IPublisher publisher,
    IDateTimeProvider dateTimeProvider,
    IOptions<OutboxOptions> options) : IProcessOutboxMessageJob
{
    private readonly JsonSerializerSettings jsonSerializer = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.All,
    };
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IPublisher _publisher = publisher;
    private readonly ILogger<ProcessOutboxMessageJob> _logger = logger;
    private readonly OutboxOptions outboxOptions = options.Value;
    public async Task ExecuteAsync()
    {
        _logger.LogInformation("Starting processing new outbox messages batch");
        // 1. Get Messages from Database
        using var connection = _dbConnectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();
        var outboxMessages = await GetOutboxMessagesAsync(connection, transaction);
        if (!outboxMessages.Any())
        {
            _logger.LogInformation("Completed message batch processing - no messages available");
            return;
        }
        // 2. Processing messages
        foreach (var outboxMessage in outboxMessages)
        {
            Exception? exception = null;
            try
            {
                IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, jsonSerializer)!;

                await _publisher.Publish(domainEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing message with id {MessageId}", outboxMessage.Id);
                exception = ex;
            }

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }

        // Save changes
        transaction.Commit();
        _logger.LogInformation("Completed outbox messages batch processing");
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(IDbConnection dbConnection, IDbTransaction transaction)
    {
        var sql = """
            SELECT Top(@BatchSize) id, content
            From outbox_messages With (UPDLOCK, ROWLOCK, READPAST)
            Where processed_on_utc is null
            Order By occurred_on_utc
            """;

        var result = await dbConnection.QueryAsync<OutboxMessageResponse>(
            sql, 
            new { outboxOptions.BatchSize }, 
            transaction);

        return result.ToList();
    }

    private async Task UpdateOutboxMessageAsync(IDbConnection dbConnection, IDbTransaction transaction, OutboxMessageResponse outboxMessage, Exception? exception)
    {
        var sql = """
            Update outbox_message
                Set processed_on_utc = @ProcessedOnUtc,
                    error = @Error
                Where id = @Id
            """;

        await dbConnection.ExecuteAsync(
                sql,
                new { 
                    ProcessedOnUtc = _dateTimeProvider.UtcNow, 
                    Error = exception?.ToString(), 
                    outboxMessage.Id 
                },
                transaction);
    }
}
internal sealed record OutboxMessageResponse(Guid Id, string Content);
