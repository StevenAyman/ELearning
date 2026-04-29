using ELearning.Api.DTOs.Instructors;
using ELearning.Api.DTOs.Users;
using FluentValidation;

namespace ELearning.Api.Validators.Instructors;

public sealed class CreateInstructorRequestValidator : AbstractValidator<CreateInstructorRequest>
{
    public CreateInstructorRequestValidator()
    {
        RuleFor(u => u.Email)
            .EmailAddress()
            .NotEmpty()
            .WithMessage("Not a valid email");

        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(10)
            .WithMessage("Password should be at least 10 characters");

        RuleFor(u => u.Username)
            .NotEmpty()
            .MinimumLength(15)
            .WithMessage("Username is should at least 15 character");

        RuleFor(u => u.City)
            .NotEmpty()
            .MinimumLength(4)
            .WithMessage("City should at least be 4 characters");

        RuleFor(u => u.FirstName)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("First name should be at least 3 characters");

        RuleFor(u => u.LastName)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("Last name should be at least 3 characters");

        RuleFor(u => u.BirthDate)
            .NotEmpty()
            .Matches(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$")
                .WithMessage("Date must follow the year-month-day pattern (yyyy-MM-dd).");
    }
}
