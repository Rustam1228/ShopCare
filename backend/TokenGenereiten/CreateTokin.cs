using backend.Core.JwtOp;
using backend.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.TokenGeneration
{
    public class CreateTokin(IOptions<JwtOptions> options)
    {
        private readonly JwtOptions _jwtOptions = options.Value;
        public string GenerationToken(User user)
        {
            Claim[] claims = [new("login", user.Login)];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SekretKey)),
                SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
                (
                signingCredentials: signingCredentials,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpiresHours)
                );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue.ToString();
        }
    }

}
