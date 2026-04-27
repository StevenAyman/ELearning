using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ELearning.Application.Users.UpdateUserEmail;
internal sealed class UpdateUserEmailCommandValidator : AbstractValidator<UpdateUserEmailCommand>
{
    public UpdateUserEmailCommandValidator()
    {
        RuleFor(u => u.OldEmail)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("The provided email is in invalid email format");

        RuleFor(u => u.NewEmail)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("The provided email is in invalid email format");
    }
}
