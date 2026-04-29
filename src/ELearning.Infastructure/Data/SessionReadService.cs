using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Sessions.DTOs;

namespace ELearning.Infastructure.Data;
public sealed class SessionReadService(IDbConnectionFactory dbConnectionFactory) : ISessionReadService
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<IReadOnlyList<SessionDto>> GetAllWithInstructorIdAsync(string instructorId, CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var sql = """
            Select
            id As Id,
            title As Title,
            description As Description,
            price As Price,
            created_on_utc As CreatedOn
            From sessions
            Where instructor_id = @InstructorId
            Order By created_on_utc

            Select 
            id As Id,
            title As Title,
            price As Price,
            published_at_utc As CreatedOn
            From exams
            Where instructor_id = @InstructorId
            Order By published_at_utc
            """;

        //var result = await connection.QueryAsync<SessionDto>(new CommandDefinition(
        //    sql,
        //    new { InstructorId = instructorId },
        //    cancellationToken: cancellationToken));

        using var multi = await connection.QueryMultipleAsync(new CommandDefinition(
            sql,
            new { InstructorId = instructorId },
            cancellationToken:  cancellationToken));

        var sessions = await multi.ReadAsync<SessionDto>();
        var exams = await multi.ReadAsync<SessionDto>();
        sessions = sessions.Concat(exams);
        sessions = sessions.OrderBy(s => s.CreatedOn);

        return sessions is null ? [] : sessions.ToList();
    }
}
