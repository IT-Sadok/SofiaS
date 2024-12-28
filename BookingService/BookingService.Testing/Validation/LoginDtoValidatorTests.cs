using BookingService.Application.DTOs;
using BookingService.Application.Validation;
using FluentValidation.TestHelper;

namespace BookingService.Testing.Validation
{
    public class LoginDtoValidatorTests
    {
        private readonly LoginDtoValidator _loginValidator;

        public LoginDtoValidatorTests()
        {
            _loginValidator = new LoginDtoValidator();
        }

        [Fact]
        public void LoginValidator_ShouldSucceed_WhenDataValid()
        {
            var loginDto = new LoginDto { Email = "test@example.com", Password = "Password123*" };

            var result = _loginValidator.TestValidate(loginDto);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("", "Password123*", "Email is required.")]
        [InlineData("invalid-email", "Password123*", "Invalid email format")]
        [InlineData("this.email.is.way.too.long.long.long.long.long.long.long.long.long.to.be.valid@thisisaverylongdomainname.com", 
            "Password123*", "Email must not exceed 100 characters.")]
        public void LoginValidator_ShouldFail_WhenEmailInvalid(string email, string password, string errorMessage)
        {
            var loginDto = new LoginDto { Email = email, Password = password };

            var result = _loginValidator.TestValidate(loginDto);

            result.ShouldHaveValidationErrorFor(nameof(loginDto.Email))
                .WithErrorMessage(errorMessage);
        }

        [Theory]
        [InlineData("test@examle.com", "", "Password is required.")]
        [InlineData("test@examle.com", "short", "Password must be at least 6 characters long.")]
        [InlineData("test@examle.com", 
                "ThisPasswordIsWayTooLongLongLongLongLongLongLongLongLongLongLongForValidationAndShouldFailBecauseItExceedsTheAllowedCharacterLimit",
                "Password must not exceed 128 characters.")]
        [InlineData("test@examle.com", "password", "Password must contain at least one uppercase letter.")]
        [InlineData("test@examle.com", "PASSWORD", "Password must contain at least one lowercase letter.")]
        [InlineData("test@examle.com", "Password", "Password must contain at least one number.")]
        [InlineData("test@examle.com", "Password1", "Password must contain at least one special character.")]
        [InlineData("test@examle.com", "Password 1!", "Password cannot contain spaces.")]
        public void LoginValidator_ShouldFail_WhenPasswordInValid(string email, string password, string errorMessage)
        {
            var loginDto = new LoginDto { Email = email, Password = password };

            var result = _loginValidator.TestValidate(loginDto);

            result.ShouldHaveValidationErrorFor(nameof(loginDto.Password))
                .WithErrorMessage(errorMessage);
        }
    }
}
