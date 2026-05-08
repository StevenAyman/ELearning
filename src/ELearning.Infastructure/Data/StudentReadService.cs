using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Common;
using ELearning.Application.Students.DTOs;

namespace ELearning.Infastructure.Data;
internal sealed class StudentReadService(IDbConnectionFactory dbConnectionFactory) : IStudentReadService
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<StudentDto?> GetStudentByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var sql = """
            Select
            u.id AS Id,
            u.first_name AS FirstName,
            u.last_name AS LastName,
            u.email AS Email,
            u.date_of_birth AS BirthDate,
            s.wallet AS Wallet,
            lc.type AS Class,
            u.joined_on_utc AS JoinedOn
            From users u inner join students s
            On u.id = s.id
            JOIN learning_class lc
            On lc.id = s.class_id
            Where u.id = @Id
            """;

        using var connection = _dbConnectionFactory.CreateConnection();

        var result = await connection.QueryFirstOrDefaultAsync<StudentDto>(new CommandDefinition(
            sql,
            new { Id = id },
            cancellationToken: cancellationToken));

        return result;
    }

    public async Task<PaginationDto<StudentDto>> GetAllAsync(
        string? search,
        string? orderBy,
        string? classId, 
        string? subjectId, 
        string? instructorId,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        search = search?.Trim();
        orderBy = orderBy == "id" ? "st.id" : orderBy;
        var sql =$"""
            Select
            st.id AS Id,
            u.first_name AS FirstName,
            u.last_name AS LastName,
            u.date_of_birth AS BirthDate,
            u.email AS Email,
            st.wallet AS Wallet,
            lc.type AS Class,
            u.joined_on_utc AS JoinedOn
            From users u inner join students st
            On u.id = st.id
            Left Outer Join enrollments e On e.student_id = st.id
            Left Outer Join sessions s On st.id = e.session_id
            Left Outer Join learning_class lc On lc.id = st.class_id
            Where (@ClassId is NULL OR st.class_id = @ClassId)                      and 
                  (@SubjectId is NULL OR s.subject_id = @SubjectId)                 and
                  (@InstructorId is NULL OR s.instructor_id = @InstructorId)        and
                  (@Search is NULL Or u.first_name + ' ' + u.last_name Like '%' + @Search + '%')
            Order By {orderBy}
            OFFSET (@PageIndex - 1) * @PageSize ROWS
            FETCH NEXT @PageSize ROWS Only


            Select COUNT(*)
            From users u inner join students st
            On u.id = st.id
            Left Outer Join enrollments e On e.student_id = st.id
            Left Outer Join sessions s On s.id = e.session_id
            Where (@ClassId is NULL OR st.class_id = @ClassId)                 and 
                  (@SubjectId is NULL OR s.subject_id = @SubjectId)            and
                  (@InstructorId is NULL OR s.instructor_id = @InstructorId)   and
                  (@Search is NULL Or u.first_name + ' ' + u.last_name Like '%' + @Search + '%')
            """;

        using var connection = _dbConnectionFactory.CreateConnection();
        var dataReader = await connection.QueryMultipleAsync(new CommandDefinition(
            sql,
            new { ClassId = classId, SubjectId = subjectId, InstructorId = instructorId,
                Search = search, PageSize = pageSize, PageIndex = pageIndex },
            cancellationToken: cancellationToken));

        var students = await dataReader.ReadAsync<StudentDto>();

        var count = await dataReader.ReadSingleAsync<int>();

        var dto = new PaginationDto<StudentDto>
        {
            Data = students.ToList(),
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = count
        };

        return dto;
    }
}
