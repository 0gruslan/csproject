using FluentValidation;
using WebApplication1.Models.DTO;

namespace WebApplication1.Validators;

public class CreateAccessCardDtoValidator : AbstractValidator<CreateAccessCardDto>
{
    public CreateAccessCardDtoValidator()
    {
        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Card number is required")
            .MaximumLength(50).WithMessage("Card number must not exceed 50 characters");
    }
}


