using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Sessions.DTOs;

namespace ELearning.Application.Abstractions.Data;
public interface ISessionReadService
{
    Task<IReadOnlyList<SessionDto>> GetAllWithInstructorIdAsync(string instructorId, CancellationToken cancellationToken = default);
}
