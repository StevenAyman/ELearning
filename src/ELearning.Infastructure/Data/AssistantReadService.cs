using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Assistants.DTOs;

namespace ELearning.Infastructure.Data;
internal sealed class AssistantReadService(IDbConnectionFactory dbConnectionFactory) : IAssistantReadService
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<AssistantResponse?> GetAssistantByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var sql = """
            Select
            s.id As Id, u.first_name + ' ' + u.last_name As Name,
            ins.id as InstructorId, ins.first_name + ' ' + ins.last_name As Name
            From users u inner join assistants s
            On u.id = s.id
            join users ins
            on ins.id = s.instructor_id
            Where u.id = @Id
            """;

        var result = await connection.QueryAsync<AssistantResponse, InstructorWithoutRatingDto, AssistantResponse>(
            sql,
            map: (assistant, instructor) => assistant with { Instructor = instructor },
            new { Id = id }, splitOn: "InstructorId"
            );

        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<AssistantResponse>> GetAssistantsByInstructorIdAsync(string? instructorId, CancellationToken cancellationToken = default)
    {
        var sql = """
            Select
            s.id As Id,
            u.first_name + ' ' + u.last_name As Name
            From users u inner join assistants s On u.id = s.id
            Where @InstructorId is NULL OR instructor_id = @InstructorId
            """;

        using var connection = _dbConnectionFactory.CreateConnection();

        var result = await connection.QueryAsync<AssistantResponse>(new CommandDefinition(
            sql,
            new { InstructorId = instructorId },
            cancellationToken: cancellationToken));

        return result.ToList();
    }
}
