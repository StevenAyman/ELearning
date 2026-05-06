using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Classes.DTOs;
using ELearning.Domain.Classes;

namespace ELearning.Application.Abstractions.Data;
public interface IClassReadService
{
    Task<ClassDto?> GetByNameAsync(string className, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClassDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ClassWithSubjectsDto?> GetByIdAsync(string id,  CancellationToken cancellationToken = default);
    Task<bool> IsSubjectExistInClassAsync(string classId, string subjectId, CancellationToken cancellationToken = default);
}
