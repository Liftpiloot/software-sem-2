using FluentValidation;

namespace Iron_Domain;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.UserName).NotEmpty().WithMessage("Username is required");
        RuleFor(u => u.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");
        RuleFor(u => u.PasswordHash).NotEmpty().WithMessage("Password is required")
            .MinimumLength(10).WithMessage("Password must be at least 10 characters long");
        RuleFor(u => u.DateOfBirth).NotEmpty().WithMessage("Date of birth is required")
            .LessThan(DateTime.Now).WithMessage("Date of birth cannot be in the future");
        RuleFor(u => u.Weight).NotEmpty().WithMessage("Weight is required")
            .GreaterThan(0).WithMessage("Weight must be greater than 0")
            .LessThan(1000).WithMessage("Weight must be less than 1000");
    }
}