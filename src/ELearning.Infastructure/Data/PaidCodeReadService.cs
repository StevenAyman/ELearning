using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.PaidCodes.DTOs;
using ELearning.Domain.Purchases;

namespace ELearning.Infastructure.Data;
public sealed class PaidCodeReadService(IDbConnectionFactory dbConnectionFactory) : IPaidCodeReadService
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<IEnumerable<FullPaidCodeResponse>> GetAllAsync(PaidCodeStatus? status, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken = default)
    {
        var sql = """
            Select
            id AS Id,
            code AS Code,
            balance AS Balance,
            status AS Status,
            student_id AS StudentId,
            generated_at_utc AS GeneratedAtUtc,
            used_at_utc AS UsedAtUtc,
            expired_at_utc AS ExpiredAtUtc
            From paid_codes
            Where (@Status is NULL OR status = @Status) and
                  (@StartDate is NULL OR generated_at_utc between @StartDate and @EndDate)
            """;

        using var connection = _dbConnectionFactory.CreateConnection();
        var result = await connection.QueryAsync<FullPaidCodeResponse>(new CommandDefinition(
            sql,
            new { Status = status?.ToString(), StartDate = startDate, EndDate = endDate },
            cancellationToken: cancellationToken));

        return result.ToList();
    }

    public async Task<FullPaidCodeResponse?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var sql = """
            Select
            id AS Id,
            code AS Code,
            balance AS Balance,
            status AS Status,
            student_id AS StudentId,
            generated_at_utc AS GeneratedAtUtc,
            used_at_utc AS UsedAtUtc,
            expired_at_utc AS ExpiredAtUtc
            From paid_codes
            Where id = @Id
            """;

        using var connection = _dbConnectionFactory.CreateConnection();

        var result = await connection.QueryFirstOrDefaultAsync<FullPaidCodeResponse>(new CommandDefinition(
            sql,
            new {Id = id},
            cancellationToken: cancellationToken));

        return result;
    }
}
