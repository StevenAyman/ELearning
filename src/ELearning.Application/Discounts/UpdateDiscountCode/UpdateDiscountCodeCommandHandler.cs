using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Domain.Discounts;
using ELearning.Domain.Instructors;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;
using ELearning.Domain.Shared.Specifications;
using ELearning.Domain.Subjects;
using ELearning.Domain.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ELearning.Application.Discounts.UpdateDiscountCode;
internal sealed class UpdateDiscountCodeCommandHandler(
    IDiscountCodeRepository discountCodeRepository,
    IUnitOfWork unitOfWork,
    ICodeApplicableAreaRepository codeApplicableAreaRepository,
    ICodeAreasRepository codeAreasRepository,
    IServiceProvider serviceProvider,
    ILogger<UpdateDiscountCodeCommand> logger) : ICommandHandler<UpdateDiscountCodeCommand>
{
    private readonly IDiscountCodeRepository _discountCodeRepository = discountCodeRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICodeApplicableAreaRepository _codeApplicableAreaRepository = codeApplicableAreaRepository;
    private readonly ICodeAreasRepository _codeAreasRepository = codeAreasRepository;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<UpdateDiscountCodeCommand> _logger = logger;

    public async Task<Result> Handle(UpdateDiscountCodeCommand request, CancellationToken cancellationToken)
    {
        // 1.Get discount code
        var discountCode = await _discountCodeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (discountCode is null)
        {
            return Result.Failure(DiscountErrors.NotFound);
        }

        if (discountCode.Status == DiscountStatus.Expired)
        {
            return Result.Failure(DiscountErrors.ExpiredCode);
        }

        // 2. Check code applicable area
        var codeApplicableArea = await _codeApplicableAreaRepository.GetByIdAsync(request.ApplicableAreaId, cancellationToken);

        if (codeApplicableArea is null)
        {
            return Result.Failure(DiscountErrors.AreaNotFound);
        }

        // 3. Get All code area targets
        var spec = new BaseSpecifications<CodeAreas>(c => c.CodeId == request.Id);

        var codeAreas = await _codeAreasRepository.GetAllWithSpecAsync(spec, cancellationToken);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Update code
            if (request.Code != discountCode.Code)
            {
                var checkSpec = new BaseSpecifications<DiscountCode>(dc => dc.Code == request.Code);
                var isNewCodeFound = await _discountCodeRepository.GetWithSpecAsync(checkSpec, cancellationToken);
                if (isNewCodeFound is not null)
                {
                    return Result.Failure(DiscountErrors.Overlap);
                }

                discountCode.UpdateDiscountCode(request.Code);
            }

            discountCode.UpdateDiscountType(request.DiscountAmountType);

            // Update Expiration Criteria
            if (discountCode.ExpireType != request.ExpireType)
            {
                if (discountCode.ExpireType == DiscountExpirationType.LimitedCount)
                {
                    discountCode.UpdateCountLimit(null);
                    discountCode.UpdateExpireType(request.ExpireType);
                    discountCode.UpdateExpirePeriod(DateRange.Create(
                        request.StartExpirePeriod.HasValue ? request.StartExpirePeriod.Value : DateTime.UtcNow
                        , request.EndExpirePeriod.HasValue ? request.EndExpirePeriod.Value : DateTime.UtcNow));
                }
                else
                {
                    discountCode.UpdateExpirePeriod(null!);
                    discountCode.UpdateExpireType(request.ExpireType);
                    var result = discountCode.UpdateCountLimit(request.CountLimit);
                    if (result.IsFailure)
                    {
                        return result;
                    }
                }

            }
            else
            {
                if (discountCode.ExpireType == DiscountExpirationType.LimitedCount)
                {
                    discountCode.UpdateCountLimit(request.CountLimit);
                }
                else
                {
                    discountCode.UpdateExpirePeriod(DateRange.Create(
                       request.StartExpirePeriod.HasValue ? request.StartExpirePeriod.Value : DateTime.UtcNow
                       , request.EndExpirePeriod.HasValue ? request.EndExpirePeriod.Value : DateTime.UtcNow));
                }
            }

            // Update Amount
            discountCode.UpdateDiscountAmount(new Money(request.Amount));

            var targetIds = request.TargetIds.Distinct().ToArray();

            var codeAreasToRemove = codeAreas.Where(
                ca => ca.AppplicableAreaId != request.ApplicableAreaId ||
                      !targetIds.Contains(ca.TargetId)).ToList();

            _codeAreasRepository.RemoveRange(codeAreasToRemove);

            var allCodeAreasIds = codeAreas.Select(ca => ca.TargetId).ToList();

            // Get All new ids to add as targets for discount code
            var codeAreasToAdd = targetIds.Where(
                id => !allCodeAreasIds.Contains(id))
                .ToArray();

            if (!codeAreasToAdd.Any() && codeAreasToRemove.Count == codeAreas.Count && codeApplicableArea.Type.Value.ToLower() != DiscountApplicableAreas.General)
            {
                return Result.Failure(DiscountErrors.InvalidTarget);
            }

            var newCodeAreas = new List<CodeAreas>();

            if (codeApplicableArea.Type.Value.ToLower() == DiscountApplicableAreas.General)
            {
                var codeArea = new CodeAreas(discountCode.Id, codeApplicableArea.Id, null);
                newCodeAreas.Add(codeArea);
            }
            else
            {
                var isExist = IsTargetExist(_serviceProvider, codeApplicableArea.Type.Value, codeAreasToAdd);
                if (!isExist)
                {
                    return Result.Failure(DiscountErrors.InvalidTarget);
                }

                foreach (var targetId in codeAreasToAdd)
                {
                    var codeArea = new CodeAreas(discountCode.Id, request.ApplicableAreaId, targetId);

                    newCodeAreas.Add(codeArea);
                }
            }

            _codeAreasRepository.AddRange(newCodeAreas);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactoinAsync(cancellationToken);
            _logger.LogError(ex, "An Error has happened while trying to update discount code {Id}", discountCode.Id);
        }

        return Result.Success();

    }

    private bool IsTargetExist(IServiceProvider serviceProvider, string requiredTarget, string[] ids)
    {
        if (requiredTarget.ToLower() == DiscountApplicableAreas.Subject)
        {
            return serviceProvider.GetRequiredService<ISubjectRepository>().IsIdsExist(ids);
        }
        else if (requiredTarget.ToLower() == DiscountApplicableAreas.Instructor)
        {
            return serviceProvider.GetRequiredService<IUserRepository<Instructor>>().IsIdsExist(ids);
        }
        else
        {
            return serviceProvider.GetRequiredService<ISessionRepository>().IsIdsExist(ids);
        }
    }
}
