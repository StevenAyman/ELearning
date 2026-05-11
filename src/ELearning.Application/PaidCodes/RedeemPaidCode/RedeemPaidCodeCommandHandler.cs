using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Clock;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Purchases;
using ELearning.Domain.Shared;
using ELearning.Domain.Shared.Specifications;
using ELearning.Domain.Students;
using ELearning.Domain.Users;
using Microsoft.Extensions.Logging;

namespace ELearning.Application.PaidCodes.RedeemPaidCode;
internal sealed class RedeemPaidCodeCommandHandler(
    IPaidCodeRepository paidCodeRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IUserRepository<Student> studentRepository,
    ILogger<RedeemPaidCodeCommandHandler> logger) : ICommandHandler<RedeemPaidCodeCommand>
{
    private readonly IPaidCodeRepository _paidCodeRepository = paidCodeRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IUserRepository<Student> _studentRepository = studentRepository;
    private readonly ILogger<RedeemPaidCodeCommandHandler> _logger = logger;

    public async Task<Result> Handle(RedeemPaidCodeCommand request, CancellationToken cancellationToken)
    {
        var codeSpec = new BaseSpecifications<PaidCode>(c => c.Code.Equals(request.Code));
        var code = await _paidCodeRepository.GetWithSpecAsync(codeSpec, cancellationToken);

        if (code is null)
        {
            return Result.Failure(PaidCodeErrors.NotFound);
        }

        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);

        if (student is null)
        {
            return Result.Failure(UserErrors.UserNotExist);
        }

        if (code.Status == PaidCodeStatus.Expired)
        {
            return Result.Failure(PaidCodeErrors.Expired);
        }

        if (code.Status == PaidCodeStatus.Used)
        {
            return Result.Failure(PaidCodeErrors.Used);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            student.Deposite(code.Balance);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            code.UseCode(request.StudentId, _dateTimeProvider.UtcNow);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactoinAsync(cancellationToken);

            _logger.LogError(ex, "Error has been occurred while trying to charge code with id {Id} for student with id {StudentId}",
                code.Id, student.Id);

            return Result.Failure(Errors.DatabaseError);
        }

        return Result.Success();
    }
}
