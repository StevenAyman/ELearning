using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Discounts;
using ELearning.Domain.Shared;

namespace ELearning.Application.Discounts.DeleteDiscountArea;
internal sealed class DeleteDiscountAreaCommandHandler(
    ICodeApplicableAreaRepository codeApplicableAreaRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteDiscountAreaCommand>
{
    private readonly ICodeApplicableAreaRepository _codeApplicableAreaRepository = codeApplicableAreaRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteDiscountAreaCommand request, CancellationToken cancellationToken)
    {
        var area = await _codeApplicableAreaRepository.GetByIdAsync(request.Id, cancellationToken);

        if (area is null)
        {
            return Result.Failure(DiscountErrors.AreaNotFound);
        }

        _codeApplicableAreaRepository.Delete(area);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
