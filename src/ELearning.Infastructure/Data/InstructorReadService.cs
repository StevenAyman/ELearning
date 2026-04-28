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

    public async Task<IReadOnlyList<InstructorDto>> GetWithSubjectIdAsync(string subjectId, CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var sql = """
            Select
            i.id As Id,
            u.first_name + ' ' + u.last_name As Name
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
