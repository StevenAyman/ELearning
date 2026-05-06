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

    public async Task<IEnumerable<InstructorDto>> GetAllAsync(string? classId, string? subjectId, CancellationToken cancellationToken = default)
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
            left outer join classes_subjects_instructors csi
            On csi.instructor_id = u.id
            Where (@ClassId is NULL OR csi.class_id = @ClassId) and (@SubjectId is NULL OR csi.subject_id = @SubjectId)
            """;

        var instructors = await connection.QueryAsync<InstructorDto>(new CommandDefinition(
            sql,
            new {ClassId = classId, SubjectId = subjectId},
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

    public async Task<bool> IsInstructorAssignedAsync(string instructorId, string classId, string subjectId, CancellationToken cancellationToken = default)
    {
        var sql = """
            Select count(*)
            From classes_subjects_instructors
            Where instructor_id = @InstructorId and class_id = @ClassId and subject_id = @SubjectId
            """;

        var connection = _dbConnectionFactory.CreateConnection();
        var isExist = await connection.ExecuteScalarAsync<int>(new CommandDefinition(
            sql,
            new { InstructorId = instructorId, ClassId = classId, SubjectId = subjectId },
            cancellationToken: cancellationToken));

        return isExist >= 1;
    }

    public async Task<InstructorSubjectsDto?> GetInstructorWithSubjectsAsync(string instructorId, CancellationToken cancellationToken = default)
    {
        var sql = """
            Select
            id As Id,
            first_name + ' ' + last_name As Name
            From users
            Where id = @InstructorId

            Select
            c.id As ClassId,
            c.type As Class,
            s.id As SubjectId,
            s.name As Subject
            From classes_subjects_instructors csi join learning_class c
            on c.id = csi.class_id
            join subjects s
            on s.id = csi.subject_id
            Where instructor_id = @InstructorId
            """;

        using var connection = _dbConnectionFactory.CreateConnection();
        var dataReader = await connection.QueryMultipleAsync(new CommandDefinition(
            sql,
            new { InstructorId = instructorId },
            cancellationToken: cancellationToken));

        var instructor = await dataReader.ReadFirstOrDefaultAsync<InstructorSubjectsDto>();
        if (instructor is null)
        {
            return null;
        }

        instructor.ClassSubjects = await dataReader.ReadAsync<ClassSubjectToReturnDto>();

        return instructor;
    }
}
