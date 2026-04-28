using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ELearning.Application.Subjects.UpdateSubject;
internal sealed class UpdateSubjectCommandValidator : AbstractValidator<UpdateSubjectCommand>
{
    public UpdateSubjectCommandValidator()
    {
        RuleFor(s => s.NewName)
            .NotEmpty()
            .MinimumLength(4)
            .WithMessage("Subject name should be at least 4 characters")
            .Matches(@"^\D*$")
            .WithMessage("{PropertyName} must not contain any numbers.");
    }
}
