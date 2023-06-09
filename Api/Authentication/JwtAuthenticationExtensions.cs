using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Api.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Authentication
{
    public static class JwtAuthenticationExtensions
    {
        public static string GetUserToken(User user, IConfiguration configuration)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("userId", user.Id.ToString())
                },
                expires: DateTime.Now.AddDays(7),
                signingCredentials: signingCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return token;
        }
    }
}
