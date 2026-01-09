using FluentValidation;
using WebApplication1.Models.DTO;

namespace WebApplication1.Validators;

public class CreateAccessPointDtoValidator : AbstractValidator<CreateAccessPointDto>
{
    public CreateAccessPointDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Location)
            .MaximumLength(255).WithMessage("Location must not exceed 255 characters");
    }
}


