using BookingService.Application.DTOs;
using FluentValidation;

namespace BookingService.Application.Validation
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email must not exceed {MaxLength} characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(6).WithMessage("Password must be at least {MinLength} characters long.")
                .MaximumLength(128).WithMessage("Password must not exceed {MaxLength} characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.")
                .Must(p => !p.Contains(" ")).WithMessage("Password cannot contain spaces.");
        }
    }
}