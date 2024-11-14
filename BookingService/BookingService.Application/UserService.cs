using BookingService.Domain;
using Microsoft.AspNetCore.Identity;

namespace BookingService.Application
{
    public class UserService
    {
        private ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public string Authenticate(User userRequest)
        {
            var user = _userManager.FindByNameAsync(userRequest.UserName);
            if (user == null) return null;

            return _tokenService.GenerateToken();
        }
    }
}