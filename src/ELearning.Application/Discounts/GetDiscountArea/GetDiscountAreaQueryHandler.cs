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

namespace ELearning.Application.Discounts.GetDiscountArea;
internal sealed class GetDiscountAreaQueryHandler(
    IDiscountAreaReadService discountAreaReadService) : IQueryHandler<GetDiscountAreaQuery, DiscountAreaResponse>
{
    private readonly IDiscountAreaReadService _discountAreaReadService = discountAreaReadService;

    public async Task<Result<DiscountAreaResponse>> Handle(GetDiscountAreaQuery request, CancellationToken cancellationToken)
    {
        var area = await _discountAreaReadService.GetByIdAsync(request.Id, cancellationToken);

        if (area is null)
        {
            return Result<DiscountAreaResponse>.Failure(DiscountErrors.AreaNotFound);
        }

        return Result<DiscountAreaResponse>.Succuss(area);
    }
}
