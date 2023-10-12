using HagiDatabaseDomain;
using HagiRestApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HagiRestApi.Controllers
{
    public class JsonWebTokenFactory
    {
        private Settings _settings;


        public JsonWebTokenFactory(Settings settings)
        {
            _settings = settings;
        }

        // Note: Refactor to generic useage
        public string CreateJsonWebToken(User user)
        {
            var subject = CreateClaimsIdentity(user);
            var bearerKey = _settings.BearerKey;
            var minutesBeforeTokenExpires = _settings.MinutesBeforeJsonWebTokenExpires;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(bearerKey);


            var securityKey = new SymmetricSecurityKey(key);
            var securityAlgorithm = SecurityAlgorithms.HmacSha256Signature;
            var signingCredentials = new SigningCredentials(securityKey, securityAlgorithm);

            var currentDateTime = DateTime.Now;

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = subject,
                Expires = currentDateTime.AddMinutes(minutesBeforeTokenExpires),
                SigningCredentials = signingCredentials,
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }


        private ClaimsIdentity CreateClaimsIdentity(User user)
        {
            var userId = user.UserId;
            var claim = new Claim("id", userId.ToString());

            return new ClaimsIdentity(new Claim[]
            {
                claim,
            });
        }
    }
}
