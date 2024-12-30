using AutoMapper;
using BookingService.Application.Abstract;
using BookingService.Application.DTOs;
using BookingService.Application.Mapping;
using BookingService.Application.Services;
using BookingService.Domain.Constants;
using BookingService.Domain.Interfaces;
using BookingService.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BookingService.Testing
{
    public class UserServiceTests
    {
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IUserManager> _mockCustomUserManager;
        private readonly Mock<IRoleManager> _mockCustomRoleManager;
        private readonly UserService _userService;

        private readonly RegisterDto _registerDto = new RegisterDto { Username = "test", Email = "test@example.com", Password = "Password123*", Role = "User" };
        private readonly LoginDto _loginDto = new LoginDto { Email = "test@example.com", Password = "Password123*" };
        private readonly User _user = new User { Email = "test@example.com", PasswordHash = "Password123*" };

        public UserServiceTests()
        {
            _mockTokenService = new Mock<ITokenService>();
            _mockCustomUserManager = new Mock<IUserManager>();
            _mockCustomRoleManager = new Mock<IRoleManager>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));

            _userService = new UserService(_mockTokenService.Object, _mockCustomUserManager.Object, _mockCustomRoleManager.Object, mapper);
        }
        
        #region Login
        [Fact]
        public async Task LoginAsync_ShouldReturnJwtToken_WhenEmailAndPasswordAreValid()
        {
            string validJwt = "validJwt";
            var roles = new List<string>{ Roles.Admin};

            _mockCustomUserManager.Setup(um => um.FindByEmailAsync(_loginDto.Email))
                .ReturnsAsync(_user);

            _mockCustomUserManager.Setup(um => um.CheckPasswordAsync(_user, _loginDto.Password))
                .ReturnsAsync(true);

            _mockCustomUserManager.Setup(um => um.GetRolesAsync(_user))
                .ReturnsAsync(roles);

            _mockTokenService.Setup(tokenService => tokenService.GenerateToken(_user, roles))
                .ReturnsAsync(validJwt);

            var result = await _userService.LoginAsync(_loginDto);

            result.Should().Be(validJwt);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenPasswordIsInvalid()
        {
            _mockCustomUserManager.Setup(um => um.FindByEmailAsync(_loginDto.Email))
                .ReturnsAsync(_user);

            _mockCustomUserManager.Setup(um => um.CheckPasswordAsync(_user, _loginDto.Password))
                .ReturnsAsync(false);

            var result = await _userService.LoginAsync(_loginDto);

            result.Should().BeNull();
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenUserNotFound()
        {
            _mockCustomUserManager.Setup(um => um.FindByEmailAsync(_loginDto.Email))
                .ReturnsAsync(null as User);

            var result = await _userService.LoginAsync(_loginDto);

            result.Should().BeNull();
        }

        [Fact]
        public async Task LoginAsync_ShouldFindUserByEmail()
        {
            var result = await _userService.LoginAsync(_loginDto);

            _mockCustomUserManager.Verify(um => um.FindByEmailAsync(_loginDto.Email), Times.Once());
        }

        [Fact]
        public async Task LoginAsync_ShouldCheckPassword_WhenUserFound()
        {
            _mockCustomUserManager.Setup(um => um.FindByEmailAsync(_loginDto.Email))
                .ReturnsAsync(_user);

            var result = await _userService.LoginAsync(_loginDto);

            _mockCustomUserManager.Verify(um => um.CheckPasswordAsync(_user, _loginDto.Password), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldGetRoles()
        {
            _mockCustomUserManager.Setup(um => um.FindByEmailAsync(_loginDto.Email))
                .ReturnsAsync(_user);

            _mockCustomUserManager.Setup(um => um.CheckPasswordAsync(_user, _loginDto.Password))
                .ReturnsAsync(true);

            var result = await _userService.LoginAsync(_loginDto);

            _mockCustomUserManager.Verify(um => um.GetRolesAsync(_user), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldGenerateToken()
        {
            var roles = new List<string> { Roles.Admin };

            _mockCustomUserManager.Setup(um => um.FindByEmailAsync(_loginDto.Email))
                .ReturnsAsync(_user);

            _mockCustomUserManager.Setup(um => um.CheckPasswordAsync(_user, _loginDto.Password))
                .ReturnsAsync(true);

            _mockCustomUserManager.Setup(um => um.GetRolesAsync(_user))
                .ReturnsAsync(roles);

            var result = await _userService.LoginAsync(_loginDto);

            _mockTokenService.Verify(tokenService => tokenService.GenerateToken(_user, roles), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldNotCheckPassword_WhenUserNotFound()
        {
            _mockCustomUserManager.Setup(um => um.FindByEmailAsync(_loginDto.Email))
                .ReturnsAsync(null as User);

            var result = await _userService.LoginAsync(_loginDto);

            _mockCustomUserManager.Verify(um => um.CheckPasswordAsync(_user, _loginDto.Password), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_ShouldNotGetRoles_WhenUserNotFound()
        {
            _mockCustomUserManager.Setup(um => um.FindByEmailAsync(_loginDto.Email))
                .ReturnsAsync(_user);

            _mockCustomUserManager.Setup(um => um.CheckPasswordAsync(_user, _loginDto.Password))
                .ReturnsAsync(false);

            var result = await _userService.LoginAsync(_loginDto);

            _mockCustomUserManager.Verify(um => um.GetRolesAsync(_user), Times.Never);
        }

        #endregion

        #region Register
        [Fact]
        public async Task RegisterAsync_ShouldReturnSuccess_WhenValidData()
        {
            _mockCustomUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), _registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _mockCustomRoleManager.Setup(rm => rm.RoleExistsAsync(_registerDto.Role))
                .ReturnsAsync(true);

            _mockCustomUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), _registerDto.Role))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _userService.RegisterAsync(_registerDto);

            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFailed_WhenCreateFailed()
        {
            _mockCustomUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), _registerDto.Password))
                .ReturnsAsync(IdentityResult.Failed());

            var result = await _userService.RegisterAsync(_registerDto);

            result.Succeeded.Should().BeFalse();
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFailed_WhenRoleNotExist()
        {
            _mockCustomUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), _registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _mockCustomRoleManager.Setup(rm => rm.RoleExistsAsync(_registerDto.Role))
                .ReturnsAsync(false);

            var result = await _userService.RegisterAsync(_registerDto);

            result.Succeeded.Should().BeFalse();
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFailed_WhenAddToRoleFailed()
        {
            _mockCustomUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), _registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _mockCustomRoleManager.Setup(rm => rm.RoleExistsAsync(_registerDto.Role))
                .ReturnsAsync(true);

            _mockCustomUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), _registerDto.Role))
                .ReturnsAsync(IdentityResult.Failed());

            var result = await _userService.RegisterAsync(_registerDto);

            result.Succeeded.Should().BeFalse();
        }

        public async Task RegisterAsync_ShouldCreateUser()
        {
            var result = await _userService.RegisterAsync(_registerDto);

            _mockCustomUserManager.Verify(um => um.CreateAsync(It.IsAny<User>(), _registerDto.Password), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldCheckRoleExisting_WhenCreatingUserSucceed()
        {
            _mockCustomUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), _registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _userService.RegisterAsync(_registerDto);

            _mockCustomRoleManager.Verify(rm => rm.RoleExistsAsync(_registerDto.Role), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldAddToRole_WhenRoleExists()
        {
            _mockCustomUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), _registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _mockCustomRoleManager.Setup(rm => rm.RoleExistsAsync(_registerDto.Role))
                .ReturnsAsync(true);

            var result = await _userService.RegisterAsync(_registerDto);

            _mockCustomUserManager.Verify(um => um.AddToRoleAsync(It.IsAny<User>(), _registerDto.Role), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldNotCheckRoleExisting_WhenCreatingUserFailed()
        {
            _mockCustomUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), _registerDto.Password))
                .ReturnsAsync(IdentityResult.Failed());

            var result = await _userService.RegisterAsync(_registerDto);

            _mockCustomRoleManager.Verify(rm => rm.RoleExistsAsync(_registerDto.Role), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_ShouldNotAddToRole_WhenRoleNotExist()
        {
            _mockCustomUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), _registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _mockCustomRoleManager.Setup(rm => rm.RoleExistsAsync(_registerDto.Role))
                .ReturnsAsync(false);

            var result = await _userService.RegisterAsync(_registerDto);

            _mockCustomUserManager.Verify(um => um.AddToRoleAsync(It.IsAny<User>(), _registerDto.Role), Times.Never);
        }

        #endregion
    }
}