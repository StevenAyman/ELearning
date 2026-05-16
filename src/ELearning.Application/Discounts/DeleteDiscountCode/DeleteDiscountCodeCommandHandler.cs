using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Discounts;
using ELearning.Domain.Shared;

namespace ELearning.Application.Discounts.DeleteDiscountCode;
internal sealed class DeleteDiscountCodeCommandHandler(
    IDiscountCodeRepository discountCodeRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteDiscountCodeCommand>
{
    private readonly IDiscountCodeRepository _discountCodeRepository = discountCodeRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteDiscountCodeCommand request, CancellationToken cancellationToken)
    {
        var code = await _discountCodeRepository.GetByIdAsync(request.Id, cancellationToken);

        if (code is null)
        {
            return Result.Failure(DiscountErrors.NotFound);
        }

        _discountCodeRepository.Delete(code);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
