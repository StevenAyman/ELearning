using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Discounts.DTOs;

namespace ELearning.Infastructure.Data;
internal sealed class DiscountAreaReadService(IDbConnectionFactory dbConnectionFactory) : IDiscountAreaReadService
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<IEnumerable<DiscountAreaResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sql = """
            Select
            id AS Id,
            type AS Area
            From code_applicable_areas
            """;

        using var connection = _dbConnectionFactory.CreateConnection();

        var result = await connection.QueryAsync<DiscountAreaResponse>(new CommandDefinition(
            sql,
            cancellationToken: cancellationToken));

        return result.ToList();
    }

    public async Task<DiscountAreaResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var sql = """
            Select
            id AS Id,
            type AS Area
            From code_applicable_areas
            Where id = @Id
            """;

        using var connection = _dbConnectionFactory.CreateConnection();

        var area = await connection.QueryFirstOrDefaultAsync<DiscountAreaResponse>(new CommandDefinition(
            sql,
            new { Id = id },
            cancellationToken: cancellationToken));

        return area;
    }

    public async Task<DiscountAreaResponse?> GetByNameAsync(string area, CancellationToken cancellationToken = default)
    {
        var sql = """
            Select
            id AS Id,
            type AS Area
            From code_applicable_areas
            Where type = @Area
            """;

        using var connection = _dbConnectionFactory.CreateConnection();

        var res = await connection.QueryFirstOrDefaultAsync<DiscountAreaResponse>(new CommandDefinition(
            sql,
            new { Area = area},
            cancellationToken: cancellationToken));

        return res;
    }

    public async Task<bool> IsExistAsync(string area, CancellationToken cancellationToken = default)
    {
        var sql = """
            Select count(*)
            From code_applicable_areas
            Where type = @Area
            """;

        using var connection = _dbConnectionFactory.CreateConnection();

        var count = await connection.ExecuteScalarAsync<int>(new CommandDefinition(
            sql,
            new { Area = area },
            cancellationToken: cancellationToken));

        return count >= 1;
    }
}
