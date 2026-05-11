using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ELearning.Application.PaidCodes.GeneratePaidCodes;
public sealed class GeneratePaidCodesCommandValidator : AbstractValidator<GeneratePaidCodesCommand>
{
    public GeneratePaidCodesCommandValidator()
    {
        RuleFor(c => c.Count)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Count should be 1 or more");

        RuleFor(c => c.Balance)
            .GreaterThanOrEqualTo(40)
            .WithMessage("Code balance can't be less than 40 L.E.");
    }
}
