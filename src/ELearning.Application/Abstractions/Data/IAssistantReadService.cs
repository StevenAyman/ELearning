using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Assistants.DTOs;

namespace ELearning.Application.Abstractions.Data;
public interface IAssistantReadService
{
    Task<AssistantResponse?> GetAssistantByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AssistantResponse>> GetAssistantsByInstructorIdAsync(string? instructorId, CancellationToken cancellationToken = default);
}
