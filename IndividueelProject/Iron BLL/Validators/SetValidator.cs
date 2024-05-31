using FluentValidation;

namespace Iron_Domain;

public class SetValidator : AbstractValidator<Set>
{
    public SetValidator()
    {
        RuleFor(s => s.Weight).NotEmpty().WithMessage("Weight is required")
            .GreaterThan(0).WithMessage("Weight must be greater than 0")
            .LessThan(1000).WithMessage("Weight must be less than 1000");
        RuleFor(s => s.Reps).NotEmpty().WithMessage("Reps is required")
            .GreaterThan(0).WithMessage("Reps must be greater than 0")
            .LessThan(100).WithMessage("Reps must be less than 100");
    }
}