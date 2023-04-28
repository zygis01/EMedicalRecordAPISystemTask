using EMedicalRecordAPISystemTask.Interfaces;
using EMedicalRecordAPISystemTask.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMedicalRecordAPISystemTask.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetJwtToken(string username)
        {
            var jwtOptions = new JwtOptions(_configuration);
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: jwtOptions.ValidIssuer,
                audience: jwtOptions.ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(jwtOptions.ExpirationHours),
                signingCredentials: signinCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
