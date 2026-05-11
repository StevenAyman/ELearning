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

namespace ELearning.Application.PaidCodes.GetAllPaidCodes;
internal sealed class GetAllPaidCodesQueryHandler(
    IPaidCodeReadService paidCodeReadService) : IQueryHandler<GetAllPaidCodesQuery, IEnumerable<FullPaidCodeResponse>>
{
    private readonly IPaidCodeReadService _paidCodeReadService = paidCodeReadService;

    public async Task<Result<IEnumerable<FullPaidCodeResponse>>> Handle(GetAllPaidCodesQuery request, CancellationToken cancellationToken)
    {
        var codes = await _paidCodeReadService.GetAllAsync(request.Status, request.StartDate, request.EndDate, cancellationToken);

        return Result<IEnumerable<FullPaidCodeResponse>>.Succuss(codes);
    }
}
