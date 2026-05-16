using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Clock;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Abstractions.Messaging;
using ELearning.Application.Discounts.DTOs;
using ELearning.Domain.Discounts;
using ELearning.Domain.Instructors;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;
using ELearning.Domain.Shared.Specifications;
using ELearning.Domain.Subjects;
using ELearning.Domain.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ELearning.Application.Discounts.CreateDiscountCode;
internal sealed class CreateDiscountCodeCommandHandler(
    IDiscountCodeRepository discountCodeRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IDiscountCodeReadService discountCodeReadService,
    IDiscountAreaReadService discountAreaReadService,
    IServiceProvider serviceProvider,
    ICodeAreasRepository codeAreasRepository,
    ILogger<CreateDiscountCodeCommandHandler> logger) : ICommandHandler<CreateDiscountCodeCommand, DiscountCodeResponseWithTargets>
{
    private readonly IDiscountCodeRepository _discountCodeRepository = discountCodeRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IDiscountCodeReadService _discountCodeReadService = discountCodeReadService;
    private readonly IDiscountAreaReadService _discountAreaReadService = discountAreaReadService;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ICodeAreasRepository _codeAreasRepository = codeAreasRepository;
    private readonly ILogger<CreateDiscountCodeCommandHandler> _logger = logger;

    public async Task<Result<DiscountCodeResponseWithTargets>> Handle(CreateDiscountCodeCommand request, CancellationToken cancellationToken)
    {
        var spec = new BaseSpecifications<DiscountCode>(c => c.Code == request.Code);
        var isFound = await _discountCodeRepository.GetWithSpecAsync(spec, cancellationToken);

        if (isFound is not null)
        {
            return Result<DiscountCodeResponseWithTargets>.Failure(DiscountErrors.Overlap);
        }

        // Get Discount applicable area
        var area = await _discountAreaReadService.GetByIdAsync(request.DiscountAreaId, cancellationToken);
        if (area is null)
        {
            return Result<DiscountCodeResponseWithTargets>.Failure(DiscountErrors.AreaNotFound);
        }

        var discountCodeId = $"dc_{Guid.CreateVersion7()}";

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var discountBuilder = DiscountCode.DiscountCodeBuilder.CreateBuilder()
               .SetId(discountCodeId)
               .WithCode(request.Code)
               .WithDiscountAmountType(request.DiscountType)
               .WithDiscountAmount(new Money((decimal)request.Amount))
               .WithExpirationType(request.ExpireType);

            DiscountCode discountCode;

            if (request.ExpireType == DiscountExpirationType.Period)
            {
                var date = DateRange.Create(request.ExpirePeriodStart.HasValue ? request.ExpirePeriodStart.Value : DateTime.UtcNow
                    , request.ExpirePeriodEnd.HasValue ? request.ExpirePeriodEnd.Value : DateTime.UtcNow);
                discountCode = discountBuilder.WithExpirePeriod(date)
                    .Build(_dateTimeProvider.UtcNow);
            }
            else
            {
                discountCode = discountBuilder.WithCountLimit(request.CountLimit.HasValue ? request.CountLimit.Value : 0)
                    .Build(_dateTimeProvider.UtcNow);
            }

            _discountCodeRepository.Add(discountCode);

            var codeAreas = new List<CodeAreas>();

            if (area.Area.ToLower() != DiscountApplicableAreas.General)
            {
                if (request.TargetIds is null || !request.TargetIds.Any())
                {
                    return Result<DiscountCodeResponseWithTargets>.Failure(DiscountErrors.InvalidTarget);
                }
                // Sessions Instructors Subjects             

                var isTargetsExist = IsTargetExist(_serviceProvider, area.Area, request.TargetIds);

                if (!isTargetsExist)
                {
                    return Result<DiscountCodeResponseWithTargets>.Failure(DiscountErrors.InvalidTarget);
                }

                // Add Each target to discount code
                var targets = request.TargetIds.Distinct();

                foreach (var targetId in targets)
                {
                    var codeArea = new CodeAreas(discountCode.Id, area.Id, targetId);
                    codeAreas.Add(codeArea);
                }

            }
            else
            {
                var codeArea = new CodeAreas(discountCode.Id, area.Id, null);
                codeAreas.Add(codeArea);
            }

            _codeAreasRepository.AddRange(codeAreas);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong while tring to add discount code with its targets");
            await _unitOfWork.RollbackTransactoinAsync(cancellationToken);
        }
       

        var discountCodeToReturn = await _discountCodeReadService.GetByIdAsync(discountCodeId, cancellationToken);

        if (discountCodeToReturn is null)
        {
            return Result<DiscountCodeResponseWithTargets>.Failure(Errors.DatabaseError);
        }

        return Result<DiscountCodeResponseWithTargets>.Succuss(discountCodeToReturn);
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
