using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Common;
using ELearning.Application.Students.DTOs;

namespace ELearning.Application.Abstractions.Data;
public interface IStudentReadService
{
    Task<StudentDto?> GetStudentByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<PaginationDto<StudentDto>> GetAllAsync(
        string? search,
        string? orderBy,
        string? classId,
        string? subjectId,
        string? instructorId,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default);
}
