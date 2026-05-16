using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Discounts;
using FluentValidation;

namespace ELearning.Application.Discounts.UpdateDiscountCode;
public class UpdateDiscountCodeCommandValidator : AbstractValidator<UpdateDiscountCodeCommand>
{
    public UpdateDiscountCodeCommandValidator()
    {
        RuleFor(c => c.Code)
            .NotEmpty()
            .MinimumLength(4)
            .MaximumLength(20)
            .WithMessage("Invalid code. Valid code should be atleast 4 character and atmost 20");

        RuleFor(c => c.Amount)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(c => c.DiscountAmountType)
            .IsInEnum()
            .WithMessage("Invalid discount type. discount type should be (FixedAmount OR Percentage");

        When(c => c.DiscountAmountType == DiscountAmountType.Percentage, () =>
        {
            RuleFor(c => c.Amount)
            .InclusiveBetween(0, 100)
            .WithMessage("Error amount should be between 0 and 100");
        });

        RuleFor(c => c.ExpireType)
            .IsInEnum()
            .WithMessage("Invalid expiration type. Expire type should be (Period OR LimitedCount)");

        When(c => c.ExpireType == DiscountExpirationType.LimitedCount, () =>
        {
            RuleFor(c => c.CountLimit)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Sorry you should specifiy only the count limit");

            RuleFor(c => c.StartExpirePeriod)
            .Null()
            .WithMessage("Invalid operation. You shouldn't set expire period start if you chosed limited count as expire type");

            RuleFor(c => c.EndExpirePeriod)
            .Null()
            .WithMessage("Invalid operation. You shouldn't set expire period end if you chosed count limit as expire type");
        });

        When(c => c.ExpireType == DiscountExpirationType.Period, () =>
        {
            RuleFor(c => c.CountLimit)
            .Null()
            .WithMessage("Invalid operation. You shouldn't set count limit if you chosed period as expire type");

            RuleFor(c => c.StartExpirePeriod)
            .NotNull()
            .WithMessage("Error expire period start can't be null")
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Invalid date. start expire period can't be in the past.");

            RuleFor(c => c.EndExpirePeriod)
            .NotNull()
            .WithMessage("Error expire period end can't be null")
            .GreaterThan(c => c.StartExpirePeriod)
            .WithMessage("Invalid date. End expire period can't be before expire period start.");
        });

    }
}
