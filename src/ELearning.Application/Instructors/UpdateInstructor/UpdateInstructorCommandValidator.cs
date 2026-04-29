using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ELearning.Application.Instructors.UpdateInstructor;
internal sealed class UpdateInstructorCommandValidator : AbstractValidator<UpdateInstructorCommand>
{
    public UpdateInstructorCommandValidator()
    {
        RuleFor(i => i.Bio)
            .NotEmpty()
            .MinimumLength(100)
            .WithMessage("Bio should be at least 100 characters");
    }
}
