using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Instructors.DTOs;

namespace ELearning.Infastructure.Data;
public sealed class InstructorReadService(IDbConnectionFactory dbConnectionFactory) : IInstructorReadService
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<IEnumerable<InstructorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var sql = """
            Select
            u.id As Id,
            u.first_name + ' ' + u.last_name As Name,
            i.rating_average As Rating,
            i.rating_count As RatingCount
            From users u inner join instructors i
            On u.id = i.id
            """;

        var instructors = await connection.QueryAsync<InstructorDto>(new CommandDefinition(
            sql,
            cancellationToken: cancellationToken));

        return instructors is null ? [] : instructors.ToList();
    }

    public async Task<InstructorWithSessionsDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var sql = """
            Select
            i.id As Id,
            u.first_name + ' ' + u.last_name As Name,
            i.bio As Bio,
            i.rating_average As Rating,
            i.rating_count As RatingCount
            From instructors i join users u
            On u.id = i.id
            Where i.id = @Id
            """;

        var instructor = await connection.QueryFirstOrDefaultAsync<InstructorWithSessionsDto>(new CommandDefinition(
            sql,
            new { Id = id },
            cancellationToken: cancellationToken));

        return instructor;
    }

    public async Task<IEnumerable<InstructorDto>> GetWithSubjectIdAsync(string subjectId, CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var sql = """
            Select
            i.id As Id,
            u.first_name + ' ' + u.last_name As Name,
            i.rating_average As Rating,
            i.rating_count As RatingCount
            From users u inner join instructors i
            On u.id = i.id
            Where i.subject_id = @SubjectId
            """;

        var result = await connection.QueryAsync<InstructorDto>(
            new CommandDefinition(
                sql,
                new { SubjectId = subjectId }, cancellationToken: cancellationToken));

        
        return result is null ? [] : result.ToList();

    }
}
