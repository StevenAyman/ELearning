using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Discounts;
using ELearning.Domain.Shared;

namespace ELearning.Application.Discounts.UpdateDiscountArea;
internal sealed class UpdateDiscountAreaCommandHandler(
    ICodeApplicableAreaRepository codeApplicableAreaRepository,
    IUnitOfWork unitOfWork,
    IDiscountAreaReadService discountAreaReadService) : ICommandHandler<UpdateDiscountAreaCommand>
{
    private readonly ICodeApplicableAreaRepository _codeApplicableAreaRepository = codeApplicableAreaRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDiscountAreaReadService _discountAreaReadService = discountAreaReadService;

    public async Task<Result> Handle(UpdateDiscountAreaCommand request, CancellationToken cancellationToken)
    {
        var area = await _codeApplicableAreaRepository.GetByIdAsync(request.Id, cancellationToken);

        if (area is null)
        {
            return Result.Failure(DiscountErrors.AreaNotFound);
        }

        var isFound = await _discountAreaReadService.IsExistAsync(request.NewAreaName, cancellationToken);

        if (isFound)
        {
            return Result.Failure(DiscountErrors.AreaExists);
        }

        area.Update(new TypeName(request.NewAreaName));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
