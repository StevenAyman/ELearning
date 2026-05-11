using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Clock;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Purchases;
using ELearning.Domain.Shared;
using ELearning.Domain.Shared.Specifications;

namespace ELearning.Application.PaidCodes.ExpirePaidCode;
internal sealed class ExpirePaidCodeCommandHandler(
    IPaidCodeRepository paidCodeRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<ExpirePaidCodeCommand>
{
    private readonly IPaidCodeRepository _paidCodeRepository = paidCodeRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public async Task<Result> Handle(ExpirePaidCodeCommand request, CancellationToken cancellationToken)
    {
        var codeSpec = new BaseSpecifications<PaidCode>(c => c.Code == request.Code);
        var code = await _paidCodeRepository.GetWithSpecAsync(codeSpec, cancellationToken);

        if (code is null)
        {
            return Result.Failure(PaidCodeErrors.NotFound);
        }

        if (code.Status == PaidCodeStatus.Used)
        {
            return Result.Failure(PaidCodeErrors.Used);
        }

        if (code.Status == PaidCodeStatus.Expired)
        {
            return Result.Success();
        }

        code.ExpireCode(_dateTimeProvider.UtcNow);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0)
        {
            return Errors.DatabaseError;
        }

        return Result.Success();

    }
}
