using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Assistants.DTOs;
using ELearning.Domain.Shared;
using ELearning.Domain.Users;

namespace ELearning.Application.Assistants.GetAssistant;
internal sealed class GetAssistantQueryHandler(
    IAssistantReadService assistantReadService) : IQueryHandler<GetAssistantQuery, AssistantResponse>
{
    private readonly IAssistantReadService _assistantReadService = assistantReadService;

    public async Task<Result<AssistantResponse>> Handle(GetAssistantQuery request, CancellationToken cancellationToken)
    {
        var assistant = await _assistantReadService.GetAssistantByIdAsync(request.Id, cancellationToken);

        if (assistant is null)
        {
            return Result<AssistantResponse>.Failure(UserErrors.UserNotExist);
        }

        return assistant;
    }
}
