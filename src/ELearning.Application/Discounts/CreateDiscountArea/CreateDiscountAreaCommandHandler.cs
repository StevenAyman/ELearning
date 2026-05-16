using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Discounts;
using ELearning.Domain.Shared;

namespace ELearning.Application.Discounts.CreateDiscountArea;
internal sealed class CreateDiscountAreaCommandHandler(
    ICodeApplicableAreaRepository codeApplicableArea,
    IDiscountAreaReadService discountAreaReadService,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateDiscountAreaCommand, int>
{
    private readonly ICodeApplicableAreaRepository _codeApplicableArea = codeApplicableArea;
    private readonly IDiscountAreaReadService _discountAreaReadService = discountAreaReadService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<int>> Handle(CreateDiscountAreaCommand request, CancellationToken cancellationToken)
    {
        var isExist = await _discountAreaReadService.IsExistAsync(request.Area, cancellationToken);

        if (isExist)
        {
            return Result<int>.Failure(DiscountErrors.AreaExists);
        }

        var area = new CodeApplicableArea(new TypeName(request.Area));

        _codeApplicableArea.Add(area);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var res = await _discountAreaReadService.GetByNameAsync(request.Area, cancellationToken);

        if (res is null)
        {
            return Result<int>.Failure(Errors.DatabaseError);
        }

        return Result<int>.Succuss(res.Id);
    }
}
