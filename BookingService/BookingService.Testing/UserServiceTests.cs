using AutoMapper;
using BookingService.Application;
using BookingService.Application.Abstract;
using BookingService.Application.DTOs;
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

        public UserServiceTests()
        {
            _mockTokenService = new Mock<ITokenService>();
            _mockCustomUserManager = new Mock<ICustomUserManager>();
            _mockCustomRoleManager = new Mock<ICustomRoleManager>();
            _mockMapper = new Mock<IMapper>();

            _userService = new UserService(_mockTokenService.Object, _mockCustomUserManager.Object, _mockCustomRoleManager.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task LoginAsync_ReturnsJwtToken_WhenEmailAndPasswordAreValid()
        {
            //Arrange
            var email = "test@example.com";
            var password = "Password123*";
            var loginDtoUser = new LoginDto { Email = email, Password = password };
            var user = new User { Email = email, PasswordHash = password };

            _mockCustomUserManager.Setup(userManager => userManager.FindByEmailAsync(loginDtoUser.Email)).ReturnsAsync(user);
            _mockCustomUserManager.Setup(userManager => userManager.CheckPasswordAsync(user, loginDtoUser.Password)).ReturnsAsync(true);
            _mockTokenService.Setup(tokenService => tokenService.GenerateToken(user)).ReturnsAsync("validJwtToken");

            //Act
            var result = await _userService.LoginAsync(loginDtoUser);

            //Assert
            result.Should().Be("validJwtToken");
            _mockCustomUserManager.Verify(userManager => userManager.FindByEmailAsync(loginDtoUser.Email), Times.Once());
            _mockCustomUserManager.Verify(userManager => userManager.CheckPasswordAsync(user, loginDtoUser.Password), Times.Once);
            _mockTokenService.Verify(tokenService => tokenService.GenerateToken(user), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenPasswordIsInvalid()
        {
            //Arrange
            var email = "test@example.com";
            var password = "WrongPassword123*";
            var loginDtoUser = new LoginDto { Email = email, Password = password };
            var user = new User { Email = email, PasswordHash = password };

            _mockCustomUserManager.Setup(userManager => userManager.FindByEmailAsync(loginDtoUser.Email)).ReturnsAsync(user);
            _mockCustomUserManager.Setup(userManager => userManager.CheckPasswordAsync(user, loginDtoUser.Password)).ReturnsAsync(false);

            //Act
            var result = await _userService.LoginAsync(loginDtoUser);

            //Assert
            result.Should().BeNull();
            _mockCustomUserManager.Verify(userManager => userManager.FindByEmailAsync(loginDtoUser.Email), Times.Once());
            _mockCustomUserManager.Verify(userManager => userManager.CheckPasswordAsync(user, loginDtoUser.Password), Times.Once);
            _mockTokenService.Verify(tokenService => tokenService.GenerateToken(user), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenUserNotFound()
        {
            //Arrange
            var email = "test@example.com";
            var password = "WrongPassword123*";
            var loginDtoUser = new LoginDto { Email = email, Password = password };
            var user = new User { Email = email, PasswordHash = password };

            _mockCustomUserManager.Setup(userManager => userManager.FindByEmailAsync(loginDtoUser.Email)).ReturnsAsync((User)null);

            //Act
            var result = await _userService.LoginAsync(loginDtoUser);

            //Assert
            result.Should().BeNull();
            _mockCustomUserManager.Verify(userManager => userManager.FindByEmailAsync(loginDtoUser.Email), Times.Once());
            _mockCustomUserManager.Verify(userManager => userManager.CheckPasswordAsync(user, loginDtoUser.Password), Times.Never);
            _mockTokenService.Verify(tokenService => tokenService.GenerateToken(user), Times.Never);
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

    }
}