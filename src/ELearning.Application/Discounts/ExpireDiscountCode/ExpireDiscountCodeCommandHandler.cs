using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Clock;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Discounts;
using ELearning.Domain.Shared;

namespace ELearning.Application.Discounts.ExpireDiscountCode;
internal sealed class ExpireDiscountCodeCommandHandler(
    IDiscountCodeRepository discountCodeRepository,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork) : ICommandHandler<ExpireDiscountCodeCommand>
{
    private readonly IDiscountCodeRepository _discountCodeRepository = discountCodeRepository;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(ExpireDiscountCodeCommand request, CancellationToken cancellationToken)
    {
        var code = await _discountCodeRepository.GetByIdAsync(request.Id, cancellationToken);

        if (code is null)
        {
            return Result.Failure(DiscountErrors.NotFound);
        }

        var result = code.ExpireCode(_dateTimeProvider.UtcNow);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
