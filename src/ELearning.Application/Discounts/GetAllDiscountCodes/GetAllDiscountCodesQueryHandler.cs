using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Discounts.DTOs;
using ELearning.Domain.Shared;

namespace ELearning.Application.Discounts.GetAllDiscountCodes;
internal sealed class GetAllDiscountCodesQueryHandler(
    IDiscountCodeReadService discountCodeReadService) : IQueryHandler<GetAllDiscountCodesQuery, IEnumerable<DiscountCodeResponse>>
{
    private readonly IDiscountCodeReadService _discountCodeReadService = discountCodeReadService;

    public async Task<Result<IEnumerable<DiscountCodeResponse>>> Handle(GetAllDiscountCodesQuery request, CancellationToken cancellationToken)
    {
        var codes = await _discountCodeReadService.GetAllAsync(
            request.Code,
            request.DiscountType,
            request.ExpireType,
            request.Status,
            cancellationToken);

        return Result<IEnumerable<DiscountCodeResponse>>.Succuss(codes);
    }
}
