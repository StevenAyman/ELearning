using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.PaidCodes.DTOs;
using ELearning.Domain.Purchases;
using ELearning.Domain.Shared;

namespace ELearning.Application.PaidCodes.GetPaidCode;
internal sealed class GetPaidCodeQueryHandler(
    IPaidCodeReadService paidCodeReadService) : IQueryHandler<GetPaidCodeQuery, FullPaidCodeResponse>
{
    private readonly IPaidCodeReadService _paidCodeReadService = paidCodeReadService;

    public async Task<Result<FullPaidCodeResponse>> Handle(GetPaidCodeQuery request, CancellationToken cancellationToken)
    {
        var code = await _paidCodeReadService.GetByIdAsync(request.Id, cancellationToken);

        if (code is null)
        {
            return Result<FullPaidCodeResponse>.Failure(PaidCodeErrors.NotFound);
        }

        return code;
    }
}
