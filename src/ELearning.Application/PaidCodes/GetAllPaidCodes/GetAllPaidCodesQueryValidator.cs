using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ELearning.Application.PaidCodes.GetAllPaidCodes;
internal sealed class GetAllPaidCodesQueryValidator : AbstractValidator<GetAllPaidCodesQuery>
{
    public GetAllPaidCodesQueryValidator()
    {
        RuleFor(c => c.Status)
            .IsInEnum()
            .When(c => c.Status.HasValue)
            .WithMessage("Code status value is invalid");

        RuleFor(c => c.StartDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(c => c.StartDate.HasValue)
            .WithMessage("Start date can't be in future");

        RuleFor(c => c.EndDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(c => c.EndDate.HasValue)
            .WithMessage("End date can't be in the future");
    }
}
