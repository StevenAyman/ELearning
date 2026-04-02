using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Discounts.DiscountCodeBuilder;
using ELearning.Domain.Discounts.Events;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Discounts;
public sealed class DiscountCode : BaseEntity
{
    private DiscountCode() { }
    private DiscountCode(
        string id,
        string code,
        Money discountAmount,
        DiscountAmountType discountAmountType,
        DiscountExpirationType discountExpirationType,
        DateTime utcNow,
        DateRange? expirePeriod = null,
        int? countLimit = null) : base(id)
    { 
        Code = code;
        DiscountType = discountAmountType;
        ExpireType = discountExpirationType;
        GeneratedAtUtc = utcNow;
        CurrentCount = 0;
        DiscountAmount = discountAmount;
        ExpirePeriod = expirePeriod;
        CountLimit = countLimit;
    }

    public string Code { get; private set; }
    public DiscountAmountType DiscountType { get; private set; }
    public DiscountExpirationType ExpireType { get; private set; }
    public Money DiscountAmount {  get; private set; }
    public int? CountLimit { get; private set; }
    public int CurrentCount { get; private set; }
    public DateRange? ExpirePeriod { get; private set; }
    public DateTime GeneratedAtUtc { get; private set; }
    public DateTime? ExpiredAtUtc { get; private set; }
    public DateTime? LastUsedAtUtc { get; private set; }

   public class DiscountCodeBuilder : 
        IId, 
        ICode, 
        IDiscountAmountType, 
        IDiscountAmount, 
        IDiscountExpirationType, 
        IDiscountExpiration, 
        IBuild
    {
        private string _id = string.Empty;
        private string _code = string.Empty;
        private DiscountAmountType _discountAmountType;
        private DiscountExpirationType _expireType;
        private Money _discountAmount = Money.Zero();
        private int? _countLimit;
        private DateRange? _expirePeriod;
        private DiscountCodeBuilder() { }

        public static IId CreateBuilder() => new DiscountCodeBuilder();

        public ICode SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ApplicationException("Invalid id");
            }
            _id = id;
            return this;
        }
        public IDiscountAmountType WithCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ApplicationException("Invalid code");
            }
            _code = code;
            return this;
        }

        public IDiscountAmount WithDiscountAmountType(DiscountAmountType discountAmountType)
        {
            if (Enum.IsDefined(discountAmountType)) {
                _discountAmountType = discountAmountType;
                return this;
            }
            throw new ApplicationException("Invalid discount amount type");
        }

        public IDiscountExpirationType WithDiscountAmount(Money discountAmount)
        {
            if (discountAmount < Money.Zero() || _discountAmountType == DiscountAmountType.Percentage && (discountAmount.Amount < 0 || discountAmount.Amount > 100))
            {
                throw new ApplicationException("Invalid discount amount");
            }

            _discountAmount = discountAmount;
            return this; 
        }
        public IDiscountExpiration WithExpirationType(DiscountExpirationType discountExpirationType)
        {
            if (!Enum.IsDefined(discountExpirationType))
            {
                throw new ApplicationException("Invalid expiration type");
            }
            _expireType = discountExpirationType;
            return this;
        }

        public IBuild WithCountLimit(int countLimit)
        {
            if (countLimit <= 0)
            {
                throw new ApplicationException("Invalid count limit. the proper count limit should be at least 1");
            }
            if (_expireType != DiscountExpirationType.LimitedCount)
            {
                throw new ApplicationException("Invalid Method you should use the one for expire period");
            }

            _countLimit = countLimit;
            return this;

        }

        public IBuild WithExpirePeriod(DateRange expirePeriod)
        {
            if (expirePeriod is null)
            {
                throw new ApplicationException("Invalid expiration period");
            }
            if (_expireType != DiscountExpirationType.Period)
            {
                throw new ApplicationException("Invalid Method you should use the one for limited count");
            }

            _expirePeriod = expirePeriod;
            return this;
        }

        public DiscountCode Build(DateTime utcNow)
        {
            return new DiscountCode(_id, _code, _discountAmount, _discountAmountType, _expireType, utcNow, _expirePeriod, _countLimit);
        }
    }

    public Result ExpireCode(DateTime utcNow)
    {
        if (ExpiredAtUtc.HasValue)
        {
            return Result.Failure(DiscountErrors.ExpiredCode);
        }

        ExpiredAtUtc = utcNow;
        return Result.Success();

    }

    public Result UseCode(DateTime utcNow)
    {
        if (ExpiredAtUtc.HasValue)
        {
            return Result.Failure(DiscountErrors.ExpiredCode);
        }

        CurrentCount++;

        RaiseDomainEvent(new DiscountCodeUsedDomainEvent(Id));
        
        LastUsedAtUtc = utcNow;
        return Result.Success();
    }

    public Result UpdateDiscountAmount(Money discountAmount)
    {
        if (ExpiredAtUtc.HasValue)
        {
            return Result.Failure(DiscountErrors.ExpiredCode);
        }


        if (discountAmount is null ||
            discountAmount <= Money.Zero())
        {
            return Result.Failure(DiscountErrors.InvalidDiscountAmount);
        }

        if (DiscountType == DiscountAmountType.Percentage &&
            (discountAmount.Amount > 100 || discountAmount.Amount <= 0))
        {
            return Result.Failure(DiscountErrors.InvalidPercentageAmount);
        }

        if (DiscountType == DiscountAmountType.FixedAmount && 
            discountAmount.Amount <= 0)
        {
            return Result.Failure(DiscountErrors.InvalidDiscountAmount);
        }

        DiscountAmount = discountAmount;
        return Result.Success();
    }

    public Result UpdateCountLimit(int countLimit)
    {
        if (ExpiredAtUtc.HasValue)
        {
            return Result.Failure(DiscountErrors.ExpiredCode);
        }


        if (ExpireType != DiscountExpirationType.LimitedCount)
        {
            return Result.Failure(DiscountErrors.InvalidExpirationCriteria);
        }

        if (countLimit <= CurrentCount)
        {
            return Result.Failure(DiscountErrors.InvalidLimitCount);
        }

        CountLimit = countLimit;
        return Result.Success();
    }

    public Result UpdateDiscountCode(string newCode)
    {
        if (ExpiredAtUtc.HasValue)
        {
            return Result.Failure(DiscountErrors.ExpiredCode);
        }

        if (string.IsNullOrWhiteSpace(newCode))
        {
            return Result.Failure(DiscountErrors.InvalidCode);
        }

        Code = newCode;
        return Result.Success();
    }

    public Result UpdateExpirePeriod(DateRange newExpirePeriod)
    {
        if (ExpiredAtUtc.HasValue)
        {
            return Result.Failure(DiscountErrors.ExpiredCode);
        }

        if (ExpireType != DiscountExpirationType.Period)
        {
            return Result.Failure(DiscountErrors.InvalidExpirationCriteria);
        }

        if (newExpirePeriod is null)
        {
            return Result.Failure(DiscountErrors.InvalidExpirePeriod);
        }

        ExpirePeriod = newExpirePeriod;
        return Result.Success();
    }

}
