using AutoMapper;
using BookingService.Application;
using BookingService.Application.Abstract;
using BookingService.Application.DTOs;
using BookingService.Application.Validation;
using BookingService.Domain;
using BookingService.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BookingService.Testing
{
    public class UserServiceTests
    {
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<ICustomUserManager> _mockCustomUserManager;
        private readonly Mock<ICustomRoleManager> _mockCustomRoleManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserService _userService;
        private readonly LoginDtoValidator _loginDtoValidator;
        private readonly RegisterDtoValidator _registerDtoValidator;

        public UserServiceTests()
        {
            _mockTokenService = new Mock<ITokenService>();
            _mockCustomUserManager = new Mock<ICustomUserManager>();
            _mockCustomRoleManager = new Mock<ICustomRoleManager>();
            _mockMapper = new Mock<IMapper>();

            _userService = new UserService(_mockTokenService.Object, _mockCustomUserManager.Object, _mockCustomRoleManager.Object, _mockMapper.Object);

            _loginDtoValidator = new LoginDtoValidator();
            _registerDtoValidator = new RegisterDtoValidator();
        }

        [Fact]
        public async Task LoginAsync_ReturnsJwtToken_WhenEmailAndPasswordAreValid()
        {
            //Arrange
            var email = "test@example.com";
            var password = "Password123*";
            var loginDtoUser = new LoginDto { Email = email, Password = password };
            var user = new User { Email = email, PasswordHash = password };
            var roles = new List<string>{ Roles.Admin };

            _mockCustomUserManager.Setup(userManager => userManager.FindByEmailAsync(loginDtoUser.Email)).ReturnsAsync(user);
            _mockCustomUserManager.Setup(userManager => userManager.CheckPasswordAsync(user, loginDtoUser.Password)).ReturnsAsync(true);
            _mockCustomUserManager.Setup(userManager => userManager.GetRolesAsync(user)).ReturnsAsync(roles);
            _mockTokenService.Setup(tokenService => tokenService.GenerateToken(user, roles)).ReturnsAsync("validJwtToken");

            //Act
            var result = await _userService.LoginAsync(loginDtoUser);

            //Assert
            result.Should().Be("validJwtToken");
            _mockCustomUserManager.Verify(userManager => userManager.FindByEmailAsync(loginDtoUser.Email), Times.Once());
            _mockCustomUserManager.Verify(userManager => userManager.CheckPasswordAsync(user, loginDtoUser.Password), Times.Once);
            _mockCustomUserManager.Verify(userManager => userManager.GetRolesAsync(user), Times.Once);
            _mockTokenService.Verify(tokenService => tokenService.GenerateToken(user, roles), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenPasswordIsInvalid()
        {
            //Arrange
            var email = "test@example.com";
            var password = "WrongPassword123*";
            var loginDtoUser = new LoginDto { Email = email, Password = password };
            var user = new User { Email = email, PasswordHash = password };
            var roles = new List<string> { Roles.Admin };

            _mockCustomUserManager.Setup(userManager => userManager.FindByEmailAsync(loginDtoUser.Email)).ReturnsAsync(user);
            _mockCustomUserManager.Setup(userManager => userManager.CheckPasswordAsync(user, loginDtoUser.Password)).ReturnsAsync(false);

            //Act
            var result = await _userService.LoginAsync(loginDtoUser);

            //Assert
            result.Should().BeNull();
            _mockCustomUserManager.Verify(userManager => userManager.FindByEmailAsync(loginDtoUser.Email), Times.Once());
            _mockCustomUserManager.Verify(userManager => userManager.CheckPasswordAsync(user, loginDtoUser.Password), Times.Once);
            _mockCustomUserManager.Verify(userManager => userManager.GetRolesAsync(user), Times.Never);
            _mockTokenService.Verify(tokenService => tokenService.GenerateToken(user, roles), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenUserNotFound()
        {
            //Arrange
            var email = "test@example.com";
            var password = "WrongPassword123*";
            var loginDtoUser = new LoginDto { Email = email, Password = password };
            var user = new User { Email = email, PasswordHash = password };
            var roles = new List<string> { Roles.Admin };

            _mockCustomUserManager.Setup(userManager => userManager.FindByEmailAsync(loginDtoUser.Email)).ReturnsAsync((User)null);

            //Act
            var result = await _userService.LoginAsync(loginDtoUser);

            //Assert
            result.Should().BeNull();
            _mockCustomUserManager.Verify(userManager => userManager.FindByEmailAsync(loginDtoUser.Email), Times.Once());
            _mockCustomUserManager.Verify(userManager => userManager.CheckPasswordAsync(user, loginDtoUser.Password), Times.Never);
            _mockCustomUserManager.Verify(userManager => userManager.GetRolesAsync(user), Times.Never);
            _mockTokenService.Verify(tokenService => tokenService.GenerateToken(user, roles), Times.Never);
        }

        [Theory]
        [InlineData("valid.email@example.com", "Valid1@password")] // Happy path
        [InlineData("", "")] // Both fields empty
        [InlineData("invalidemail", "Valid1@password")] // Invalid email format
        [InlineData("valid.email@example.com", "")] // Empty password
        [InlineData("", "Valid1@password")] // Empty email
        [InlineData("valid.email@example.com", "short")] // Password too short
        [InlineData("valid.email@example.com", "aVeryVeryLongPasswordThatExceedsTheMaximumLengthAllowed1234567890@")] // Password too long
        [InlineData("valid.email@example.com", "nouppercase1@")] // No uppercase
        [InlineData("valid.email@example.com", "NOLOWERCASE1@")] // No lowercase
        [InlineData("valid.email@example.com", "NoNumber@")] // No number
        [InlineData("valid.email@example.com", "NoSpecialCharacter1")] // No special character
        [InlineData("valid.email@example.com", "No Space1@")] // Contains spaces
        [InlineData("email@domain.com", "Pass1!word")] // Another valid happy path
        public async Task LoginAsync_ShouldNotProceed_WhenValidationFails(string email, string password)
        {
            //Arrange
            var loginDtoUser = new LoginDto { Email = email, Password = password };
            var validationResult = await _loginDtoValidator.ValidateAsync(loginDtoUser);
            var roles = new List<string> { Roles.Admin };

            if (!validationResult.IsValid)
            {
                //Assert
                _mockCustomUserManager.Verify(userManager => userManager.FindByEmailAsync(loginDtoUser.Email), Times.Never());
                _mockCustomUserManager.Verify(userManager => userManager.CheckPasswordAsync(It.IsAny<User>(), loginDtoUser.Password), Times.Never);
                _mockCustomUserManager.Verify(userManager => userManager.GetRolesAsync(It.IsAny<User>()), Times.Never);
                _mockTokenService.Verify(tokenService => tokenService.GenerateToken(It.IsAny<User>(), roles), Times.Never);

                return;
            }

            var user = new User { Email = email, PasswordHash = password };

            _mockCustomUserManager.Setup(userManager => userManager.FindByEmailAsync(loginDtoUser.Email)).ReturnsAsync(user);
            _mockCustomUserManager.Setup(userManager => userManager.CheckPasswordAsync(user, loginDtoUser.Password)).ReturnsAsync(true);
            _mockCustomUserManager.Setup(userManager => userManager.GetRolesAsync(user)).ReturnsAsync(roles);
            _mockTokenService.Setup(tokenService => tokenService.GenerateToken(user, roles)).ReturnsAsync("validJwtToken");

            //Act 
            var result = await _userService.LoginAsync(loginDtoUser);

            //Assert
            _mockCustomUserManager.Verify(userManager => userManager.FindByEmailAsync(loginDtoUser.Email), Times.Once());
            _mockCustomUserManager.Verify(userManager => userManager.CheckPasswordAsync(It.IsAny<User>(), loginDtoUser.Password), Times.Once);
            _mockCustomUserManager.Verify(userManager => userManager.GetRolesAsync(user), Times.Once);
            _mockTokenService.Verify(tokenService => tokenService.GenerateToken(It.IsAny<User>(), roles), Times.Once);

        }

        [Fact]
        public async Task RegisterAsync_ReturnsSuccess_WhenValidData()
        {
            //Arrange
            var username = "test";
            var email = "test@example.com";
            var password = "Password123*";
            var role = "User";
            var registerUserDto = new RegisterDto { Username = username, Email = email, Password = password, Role = role };
            var user = new User();

            _mockMapper.Setup(mapper => mapper.Map<User>(registerUserDto)).Returns(user);
            _mockCustomUserManager.Setup(userManager => userManager.CreateAsync(user, registerUserDto.Password)).ReturnsAsync(IdentityResult.Success);
            _mockCustomRoleManager.Setup(roleManager => roleManager.RoleExistsAsync(registerUserDto.Role)).ReturnsAsync(true);
            _mockCustomUserManager.Setup(userManager => userManager.AddToRoleAsync(user, registerUserDto.Role)).ReturnsAsync(IdentityResult.Success);

            //Act
            var result = await _userService.RegisterAsync(registerUserDto);

            //Assert
            result.Should().Be(IdentityResult.Success);
            _mockCustomUserManager.Verify(userManager => userManager.CreateAsync(user, registerUserDto.Password), Times.Once);
            _mockCustomRoleManager.Verify(roleManager => roleManager.RoleExistsAsync(registerUserDto.Role), Times.Once);
            _mockCustomUserManager.Verify(userManager => userManager.AddToRoleAsync(user, registerUserDto.Role), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsFailed_WhenCreateFailed()
        {
            //Arrange
            var username = "test";
            var email = "test@example.com";
            var password = "Password123*";
            var role = "User";
            var registerUserDto = new RegisterDto { Username = username, Email = email, Password = password, Role = role };
            var user = new User();

            _mockMapper.Setup(mapper => mapper.Map<User>(registerUserDto)).Returns(user);
            _mockCustomUserManager.Setup(userManager => userManager.CreateAsync(user, registerUserDto.Password)).ReturnsAsync(IdentityResult.Failed());

            //Act
            var result = await _userService.RegisterAsync(registerUserDto);

            //Assert
            result.Should().NotBe(IdentityResult.Success);
            _mockCustomUserManager.Verify(userManager => userManager.CreateAsync(user, registerUserDto.Password), Times.Once);
            _mockCustomRoleManager.Verify(roleManager => roleManager.RoleExistsAsync(registerUserDto.Role), Times.Never);
            _mockCustomUserManager.Verify(userManager => userManager.AddToRoleAsync(user, registerUserDto.Role), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsFailed_WhenRoleNotExist()
        {
            //Arrange
            var username = "test";
            var email = "test@example.com";
            var password = "Password123*";
            var role = "User";
            var registerUserDto = new RegisterDto { Username = username, Email = email, Password = password, Role = role };
            var user = new User();

            _mockMapper.Setup(mapper => mapper.Map<User>(registerUserDto)).Returns(user);
            _mockCustomUserManager.Setup(userManager => userManager.CreateAsync(user, registerUserDto.Password)).ReturnsAsync(IdentityResult.Success);
            _mockCustomRoleManager.Setup(roleManager => roleManager.RoleExistsAsync(registerUserDto.Role)).ReturnsAsync(false);

            //Act
            var result = await _userService.RegisterAsync(registerUserDto);

            //Assert
            result.Should().NotBe(IdentityResult.Success);
            _mockCustomUserManager.Verify(userManager => userManager.CreateAsync(user, registerUserDto.Password), Times.Once);
            _mockCustomRoleManager.Verify(roleManager => roleManager.RoleExistsAsync(registerUserDto.Role), Times.Once);
            _mockCustomUserManager.Verify(userManager => userManager.AddToRoleAsync(user, registerUserDto.Role), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsFailed_WhenAddToRoleFailed()
        {
            //Arrange
            var username = "test";
            var email = "test@example.com";
            var password = "Password123*";
            var role = "User";
            var registerUserDto = new RegisterDto { Username = username, Email = email, Password = password, Role = role };
            var user = new User();

            _mockMapper.Setup(mapper => mapper.Map<User>(registerUserDto)).Returns(user);
            _mockCustomUserManager.Setup(userManager => userManager.CreateAsync(user, registerUserDto.Password)).ReturnsAsync(IdentityResult.Success);
            _mockCustomRoleManager.Setup(roleManager => roleManager.RoleExistsAsync(registerUserDto.Role)).ReturnsAsync(true);
            _mockCustomUserManager.Setup(userManager => userManager.AddToRoleAsync(user, registerUserDto.Role)).ReturnsAsync(IdentityResult.Failed());


            //Act
            var result = await _userService.RegisterAsync(registerUserDto);

            //Assert
            result.Should().NotBe(IdentityResult.Success);
            _mockCustomUserManager.Verify(userManager => userManager.CreateAsync(user, registerUserDto.Password), Times.Once);
            _mockCustomRoleManager.Verify(roleManager => roleManager.RoleExistsAsync(registerUserDto.Role), Times.Once);
            _mockCustomUserManager.Verify(userManager => userManager.AddToRoleAsync(user, registerUserDto.Role), Times.Once);
        }

        [Theory]
        [InlineData("validUser", "valid.email@example.com", "Valid1@password", "User")] // Happy path
        [InlineData("", "valid.email@example.com", "Valid1@password", "User")] // Empty username
        [InlineData("ab", "valid.email@example.com", "Valid1@password", "User")] // Username too short
        [InlineData("ThisUsernameIsWayTooLongToBeValidForOurValidationRules", "valid.email@example.com", "Valid1@password", "User")] // Username too long
        [InlineData("Invalid!User", "valid.email@example.com", "Valid1@password", "User")] // Username with invalid characters
        [InlineData("validUser", "", "Valid1@password", "User")] // Empty email
        [InlineData("validUser", "invalid-email", "Valid1@password", "User")] // Invalid email format
        [InlineData("validUser", "valid.email@example.com", "", "User")] // Empty password
        [InlineData("validUser", "valid.email@example.com", "short", "User")] // Password too short
        [InlineData("validUser", "valid.email@example.com", "NoUpperCase1@", "User")] // Password without uppercase
        [InlineData("validUser", "valid.email@example.com", "NOLOWERCASE1@", "User")] // Password without lowercase
        [InlineData("validUser", "valid.email@example.com", "NoNumber@", "User")] // Password without number
        [InlineData("validUser", "valid.email@example.com", "NoSpecialCharacter1", "User")] // Password without special character
        [InlineData("validUser", "valid.email@example.com", "Password with space1@", "User")] // Password with spaces
        [InlineData("validUser", "valid.email@example.com", "Valid1@password", "")] // Empty role
        [InlineData("validUser", "valid.email@example.com", "Valid1@password", "InvalidRole")] // Invalid role
        [InlineData("", "", "", "")] // All fields empty
        [InlineData("validUser", "valid.email@example.com", "Valid1@password", "Admin")] // Valid with Admin role
        [InlineData("validUser", "valid.email@example.com", "Valid1@password", "Host")] // Valid with Host role
        public async Task RegisterAsync_ShouldNotProceed_WhenDataInvalid(string username, string email, string password, string role)
        {
            //Arrange
            var registerUserDto = new RegisterDto { Username = username, Email = email, Password = password, Role = role };
            var user = new User();
            var validationResult = _registerDtoValidator.Validate(registerUserDto);

            if (!validationResult.IsValid)
            {
                _mockCustomUserManager.Verify(userManager => userManager.CreateAsync(user, registerUserDto.Password), Times.Never);
                _mockCustomRoleManager.Verify(roleManager => roleManager.RoleExistsAsync(registerUserDto.Role), Times.Never);
                _mockCustomUserManager.Verify(userManager => userManager.AddToRoleAsync(user, registerUserDto.Role), Times.Never);

                return;
            }

            _mockMapper.Setup(mapper => mapper.Map<User>(registerUserDto)).Returns(user);
            _mockCustomUserManager.Setup(userManager => userManager.CreateAsync(user, registerUserDto.Password)).ReturnsAsync(IdentityResult.Success);
            _mockCustomRoleManager.Setup(roleManager => roleManager.RoleExistsAsync(registerUserDto.Role)).ReturnsAsync(true);
            _mockCustomUserManager.Setup(userManager => userManager.AddToRoleAsync(user, registerUserDto.Role)).ReturnsAsync(IdentityResult.Success);

            //Act
            var result = await _userService.RegisterAsync(registerUserDto);

            //Assert
            result.Should().Be(IdentityResult.Success);
            _mockCustomUserManager.Verify(userManager => userManager.CreateAsync(user, registerUserDto.Password), Times.Once);
            _mockCustomRoleManager.Verify(roleManager => roleManager.RoleExistsAsync(registerUserDto.Role), Times.Once);
            _mockCustomUserManager.Verify(userManager => userManager.AddToRoleAsync(user, registerUserDto.Role), Times.Once);
        }

    }
}