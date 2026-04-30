using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Assistants.DTOs;
using ELearning.Domain.Shared;

namespace ELearning.Application.Assistants.GetAllAssistants;
internal sealed class GetAllAssistantsQueryHandler(
    IAssistantReadService assistantReadService) : IQueryHandler<GetAllAssistantsQuery, IEnumerable<AssistantResponse>>
{
    private readonly IAssistantReadService _assistantReadService = assistantReadService;

    public async Task<Result<IEnumerable<AssistantResponse>>> Handle(GetAllAssistantsQuery request, CancellationToken cancellationToken)
    {
        var result = await _assistantReadService.GetAssistantsByInstructorIdAsync(request.InstructorId, cancellationToken);

        return Result<IEnumerable<AssistantResponse>>.Succuss(result);
    }
}
