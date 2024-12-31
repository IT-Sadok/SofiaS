using BookingService.Application.DTOs;
using FluentValidation;

namespace BookingService.Application.Validation
{
    public class WalletTopUpDtoValidator : AbstractValidator<WalletTopUpDto>
    {
        public WalletTopUpDtoValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than {ComparisonValue}");
        }
    }
}
