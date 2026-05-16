using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Discounts.DTOs;
using ELearning.Domain.Discounts;
using ELearning.Domain.Shared;

namespace ELearning.Application.Discounts.GetDiscountCode;
internal sealed class GetDiscountCodeQueryHandler(
    IDiscountCodeReadService discountCodeReadService) : IQueryHandler<GetDiscountCodeQuery, DiscountCodeResponseWithTargets>
{
    private readonly IDiscountCodeReadService _discountCodeReadService = discountCodeReadService;

    public async Task<Result<DiscountCodeResponseWithTargets>> Handle(GetDiscountCodeQuery request, CancellationToken cancellationToken)
    {
        var discountCode = await _discountCodeReadService.GetByIdAsync(request.Id, cancellationToken);

        if (discountCode is null)
        {
            return Result<DiscountCodeResponseWithTargets>.Failure(DiscountErrors.NotFound);
        }

        return discountCode;
    }
}
