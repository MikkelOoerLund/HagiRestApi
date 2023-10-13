using HagiDatabaseDomain;
using HagiRestApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HagiRestApi.Controllers
{
    // Note: Refactor

    [ApiController]
    [Route("authentication")]
    public class AuthenticationController : Controller
    {
        private JsonWebTokenConfiguration _jsonWebTokenConfiguration;
        private List<UserAuthenticationDTO> _registeredUsers;

        public AuthenticationController(JsonWebTokenConfiguration jsonWebTokenConfiguration)
        {
            _jsonWebTokenConfiguration = jsonWebTokenConfiguration;
            _registeredUsers = new List<UserAuthenticationDTO>();
        }


        private IActionResult GetActionResaultFromUserAuthentication(UserAuthenticationDTO userAuthentication)
        {
            if (userAuthentication == null) return BadRequest("User authentication can not be null.");
            if (userAuthentication.Salt == null) return BadRequest("Salt can not be null");
            if (userAuthentication.UserName == null) return BadRequest("User name can not be null");
            if (userAuthentication.HashPassword == null) return BadRequest("Hash password can not be null");

            return Ok();
        }


        private string CreateJsonWebTokenFromUserAuthentication(UserAuthenticationDTO userAuthentication)
        {
            var key = _jsonWebTokenConfiguration.Key;
            var subject = _jsonWebTokenConfiguration.Subject;
            var minutesBeforeExpiration = _jsonWebTokenConfiguration.MinutesBeforeJsonWebTokenExpires;

            var guid = Guid.NewGuid();
            var constructedDateTime = DateTime.UtcNow;
            var expiresDateTime = constructedDateTime.AddMinutes(minutesBeforeExpiration);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, subject),
                new Claim(JwtRegisteredClaimNames.Jti, guid.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, constructedDateTime.ToString()),
                new Claim("UserId", userAuthentication.UserId.ToString()),
                new Claim("UserName", userAuthentication.UserName),
            };

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var securityAlgorithm = SecurityAlgorithms.HmacSha256Signature;
            var signingCredentials = new SigningCredentials(securityKey, securityAlgorithm);

            var securityToken = new JwtSecurityToken(
                    _jsonWebTokenConfiguration.Issuer,
                    _jsonWebTokenConfiguration.Audience,
                    claims,
                    expires: expiresDateTime,
                    signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] UserAuthenticationDTO userAuthentication)
        {
            var actionResault = GetActionResaultFromUserAuthentication(userAuthentication);

            if (actionResault is OkResult)
            {
                var jsonWebToken = CreateJsonWebTokenFromUserAuthentication(userAuthentication);
                _registeredUsers.Add(userAuthentication);
                return Ok(jsonWebToken);
            }

            return actionResault;
        }


        [HttpPost("login")]
        public IActionResult Login(UserAuthenticationDTO userAuthentication)
        {
            var user = _registeredUsers.Find(u => u.UserName == userAuthentication.UserName);

            if (user != null && user.HashPassword == userAuthentication.HashPassword)
            {
                var jsonWebToken = CreateJsonWebTokenFromUserAuthentication(user);
                return Ok(jsonWebToken);
            }

            return Unauthorized("Invalid username or password");
        }



    }
}
