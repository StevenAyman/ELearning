using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Discounts.DTOs;
using ELearning.Domain.Shared;

namespace ELearning.Application.Discounts.GetAllDiscountAreas;
internal sealed class GetAllDiscountAreasQueryHandler(
    IDiscountAreaReadService discountAreaReadService) : IQueryHandler<GetAllDiscountAreasQuery, IEnumerable<DiscountAreaResponse>>
{
    private readonly IDiscountAreaReadService _discountAreaReadService = discountAreaReadService;

    public async Task<Result<IEnumerable<DiscountAreaResponse>>> Handle(GetAllDiscountAreasQuery request, CancellationToken cancellationToken)
    {
        var areas = await _discountAreaReadService.GetAllAsync(cancellationToken);

        return Result<IEnumerable<DiscountAreaResponse>>.Succuss(areas);
    }
}
