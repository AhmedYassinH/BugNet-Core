using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BugNetCore.Services.DataServices.Helpers
{
    public static class JwtHelpers
    {

        public static string GenerateJwtTokenFromPrinciple(ClaimsPrincipal principal, JwtOptions jwtOptions, int LifeTimeInMinutes = 0)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = principal.Claims.ToList();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(LifeTimeInMinutes == 0 ? jwtOptions.Lifetime : LifeTimeInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.SignKey)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        public static string GenerateJwtToken(User user, JwtOptions jwtOptions, int LifeTimeInMinutes = 0 )
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(LifeTimeInMinutes == 0 ? jwtOptions.Lifetime : LifeTimeInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.SignKey)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        public static User ValidateJwtToken(string token, JwtOptions jwtOptions)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SignKey)),
                ValidateIssuerSigningKey = true
            };

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                var Id = Guid.Parse(principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
                var Username = principal.Claims.First(x => x.Type == ClaimTypes.Name).Value;
                var Email = principal.Claims.First(x => x.Type == ClaimTypes.Email).Value;
                var UserRole = (Role)Enum.Parse(typeof(Role), principal.Claims.First(x => x.Type == ClaimTypes.Role).Value);
                var user = new User
                {
                    Id = Id,
                    Username = Username,
                    Email = Email,
                    UserRole = UserRole
                };

                return user;
            }
            catch (Exception)
            {
                throw new Exception("Invalid token");
            }
        }




        public static string HashPassword(string password)
        {
            // change this to work factor of 12 for production
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }

}

