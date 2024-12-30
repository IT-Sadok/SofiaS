using BookingService.Domain.Interfaces;
using BookingService.Domain.Models;
using BookingService.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingService.Infrastructure.Authentication
{
    public class JwtTokenService : ITokenService
    {
        private readonly IOptions<JwtSettings> _options;

        public JwtTokenService(IOptions<JwtSettings> options)
        {
            _options = options;
        }

        public async Task<string> GenerateToken(User user, IEnumerable<string> roles)
        {
            //payload
            var userClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            //signature
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: userClaims,
                issuer: _options.Value.Issuer,
                audience: _options.Value.Audience,
                expires: DateTime.Now.AddHours(_options.Value.ExpirationHours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}