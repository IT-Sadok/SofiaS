using BookingService.Application.DTOs;
using BookingService.Application.Validation;
using FluentValidation.TestHelper;

namespace BookingService.Testing.Validation
{
    public class RegisterDtoValidatorTests
    {
        private readonly RegisterDtoValidator _registerValidator;

        public RegisterDtoValidatorTests()
        {
            _registerValidator = new RegisterDtoValidator();
        }

        [Fact]
        public void RegisterValidator_ShouldSucceed_WhenDataValid()
        {
            var registerDto = new RegisterDto { Username = "test", Email = "test@example.com", Password = "Password123*", Role = "User" };

            var result = _registerValidator.TestValidate(registerDto);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("", "test@example.com", "Password123*", "User", "Username is required.")]
        [InlineData("ab", "test@example.com", "Password123*", "User", "Username must be at least 4 characters long.")]
        [InlineData("this_is_a_very_long_username_exceeding_fifty_characters", "test@example.com", "Password123*", "User", "Username must not exceed 50 characters.")]
        [InlineData("invalid username", "test@example.com", "Password123*", "User", "Username can only contain letters, numbers, dots, underscores, and dashes.")]
        public void RegisterValidator_ShouldFailed_WhenUsernameInvalid(string username, string email, string password, string role, string errorMessage)
        {
            var registerDto = new RegisterDto { Username = username, Email = email, Password = password, Role = role };

            var result = _registerValidator.TestValidate(registerDto);

            result.ShouldHaveValidationErrorFor(nameof(registerDto.Username))
                .WithErrorMessage(errorMessage);
        }

        [Theory]
        [InlineData("test", "", "Password123*", "User", "Email is required.")]
        [InlineData("test", "invalid-email", "Password123*", "User", "Invalid email format")]
        [InlineData("test", "this.email.is.way.too.long.long.long.long.long.long.long.long.long.to.be.valid@thisisaverylongdomainname.com",
            "Password123*", "User", "Email must not exceed 100 characters.")]
        public void RegisterValidator_ShouldFailed_WhenEmailInvalid(string username, string email, string password, string role, string errorMessage)
        {
            var registerDto = new RegisterDto { Username = username, Email = email, Password = password, Role = role };

            var result = _registerValidator.TestValidate(registerDto);

            result.ShouldHaveValidationErrorFor(nameof(registerDto.Email))
                .WithErrorMessage(errorMessage);
        }

        [Theory]
        [InlineData("test", "test@examle.com", "", "User", "Password is required.")]
        [InlineData("test", "test@examle.com", "short", "User", "Password must be at least 6 characters long.")]
        [InlineData("test", "test@examle.com",
                "ThisPasswordIsWayTooLongLongLongLongLongLongLongLongLongLongLongForValidationAndShouldFailBecauseItExceedsTheAllowedCharacterLimit",
                 "User", "Password must not exceed 128 characters.")]
        [InlineData("test", "test@examle.com", "password", "User", "Password must contain at least one uppercase letter.")]
        [InlineData("test", "test@examle.com", "PASSWORD", "User", "Password must contain at least one lowercase letter.")]
        [InlineData("test", "test@examle.com", "Password", "User", "Password must contain at least one number.")]
        [InlineData("test", "test@examle.com", "Password1", "User", "Password must contain at least one special character.")]
        [InlineData("test", "test@examle.com", "Password 1!", "User", "Password cannot contain spaces.")]
        public void RegisterValidator_ShouldFailed_WhenPasswordInvalid(string username, string email, string password, string role, string errorMessage)
        {
            var registerDto = new RegisterDto { Username = username, Email = email, Password = password, Role = role };

            var result = _registerValidator.TestValidate(registerDto);

            result.ShouldHaveValidationErrorFor(nameof(registerDto.Password))
                .WithErrorMessage(errorMessage);
        }
        [Theory]
        [InlineData("test", "test@example.com", "Password123*", "", "Role is required.")]
        [InlineData($"test", "test@example.com", "Password123*", "InvalidRole", "Role must be one of the following: Admin, Host, User")]
        public void RegisterValidator_ShouldFailed_WhenRoleInvalid(string username, string email, string password, string role, string errorMessage)
        {
            var registerDto = new RegisterDto { Username = username, Email = email, Password = password, Role = role };

            var result = _registerValidator.TestValidate(registerDto);

            result.ShouldHaveValidationErrorFor(nameof(registerDto.Role))
                .WithErrorMessage(errorMessage);
        }
    }
}
