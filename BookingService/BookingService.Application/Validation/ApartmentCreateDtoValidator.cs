using BookingService.Application.DTOs;
using FluentValidation;

namespace BookingService.Application.Validation
{
    public class ApartmentCreateDtoValidator : AbstractValidator<ApartmentCreateDto>
    {
        public ApartmentCreateDtoValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than {ComparisonValue}");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(x => x.RoomNumber)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThanOrEqualTo(1).WithMessage("{PropertyName} must be greater than or equal to {ComparisonValue}")
                .LessThan(100).WithMessage("{PropertyName} must be less than {ComparisonValue}");
        }
    }
}