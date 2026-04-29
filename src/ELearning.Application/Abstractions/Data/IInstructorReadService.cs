using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Instructors.DTOs;

namespace ELearning.Application.Abstractions.Data;
public interface IInstructorReadService
{
    Task<IEnumerable<InstructorDto>> GetWithSubjectIdAsync(string subjectId, CancellationToken cancellationToken = default);
    Task<InstructorWithSessionsDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<InstructorDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
