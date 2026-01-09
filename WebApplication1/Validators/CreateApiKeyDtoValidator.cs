using FluentValidation;
using WebApplication1.Models.DTO;

namespace WebApplication1.Validators;

public class CreateApiKeyDtoValidator : AbstractValidator<CreateApiKeyDto>
{
    public CreateApiKeyDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.ExpiresAt)
            .Must(expiresAt => !expiresAt.HasValue || expiresAt.Value > DateTime.UtcNow)
            .WithMessage("ExpiresAt must be in the future");
    }
}


