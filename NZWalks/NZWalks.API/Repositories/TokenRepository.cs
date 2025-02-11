using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NZWalks.API.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration config)
        {
            configuration = config;
        }

        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            if (user == null) return string.Empty;

            // Create Claims 
            var claims = new List<Claim>();

            // Added Email & Roles under the Claim
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in roles) 
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Created A Key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            // Created a Credentails
            var credentails = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Created Token
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(150),
                signingCredentials: credentails);

            // return Token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
