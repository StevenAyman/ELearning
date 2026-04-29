using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Subjects.GetSubject;

namespace ELearning.Infastructure.Data;
public sealed class SubjectReadService(IDbConnectionFactory dbConnectionFactory) : ISubjectReadService
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<SubjectDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        
        var sql = """
            SELECT 
            id AS Id,
            name As Name
            From subjects
            Where id = @Id
            """;
        var subject = await connection.QueryFirstOrDefaultAsync<SubjectDto>(new CommandDefinition(
            sql,
            new { Id = id },
            cancellationToken: cancellationToken));

        return subject;
    }

    public async Task<IEnumerable<SubjectDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var sql = """
            Select
            id As Id,
            name As Name
            From subjects
            """;

        var subjects = await connection.QueryAsync<SubjectDto>(
            new CommandDefinition(sql,
            cancellationToken: cancellationToken));

        if (subjects is null)
        {
            return [];
        }

        return subjects.ToList();
    }
}
