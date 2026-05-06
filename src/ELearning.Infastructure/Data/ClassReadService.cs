using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Classes.DTOs;
using ELearning.Application.Subjects.GetSubject;
using ELearning.Domain.Classes;

namespace ELearning.Infastructure.Data;
internal sealed class ClassReadService(IDbConnectionFactory dbConnectionFactory) : IClassReadService
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<IEnumerable<ClassDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sql = """
            Select 
            id As Id,
            type As Class
            From learning_class
            """;

        using var connection = _dbConnectionFactory.CreateConnection();
        var result = await connection.QueryAsync<ClassDto>(sql, cancellationToken);

        return result.ToList();
    }

    public async Task<ClassWithSubjectsDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var sql = """
            Select
            id As Id,
            type As Class
            From learning_class
            Where id = @Id

            select
            s.id As Id,
            s.name As Name
            From subjects s join classes_subjects cs
            on s.id = cs.subject_id
            Where cs.class_id = @Id
            """;

        using var connection = _dbConnectionFactory.CreateConnection();
        var multi = await connection.QueryMultipleAsync(new CommandDefinition(
            sql,
            new {Id = id},
            cancellationToken: cancellationToken));
        var data = await multi.ReadFirstOrDefaultAsync<ClassWithSubjectsDto>();
        
        if (data is null)
        {
            return null;
        }

        data.Subjects = await multi.ReadAsync<SubjectDto>();

        return data;
    }

    public async Task<ClassDto?> GetByNameAsync(string className, CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var sql = """
            Select
            id As Id,
            type As Class
            From learning_class
            Where type = @ClassName
            """;

        var result = await connection.QueryFirstOrDefaultAsync<ClassDto>(new CommandDefinition(
            sql,
            new { ClassName = className },
            cancellationToken: cancellationToken));

        return result;
    }

    public async Task<bool> IsSubjectExistInClassAsync(string classId, string subjectId, CancellationToken cancellationToken = default)
    {
        var sql = """
            Select count(*)
            From classes_subjects
            Where class_id = @ClassId and subject_id = @SubjectId
            """;

        using var connection = _dbConnectionFactory.CreateConnection();

        var count = await connection.ExecuteScalarAsync<int>(new CommandDefinition(
            sql,
            new { ClassId = classId, SubjectId = subjectId },
            cancellationToken: cancellationToken));

        return count >= 1;
    }
}
