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
        private UserConverter _userConverter;
        private UserRepository _userRepository;
        private JsonWebTokenConfiguration _jsonWebTokenConfiguration;


        public AuthenticationController(UserConverter userConverter, UserRepository userRepository, JsonWebTokenConfiguration jsonWebTokenConfiguration)
        {
            _userConverter = userConverter;
            _userRepository = userRepository;
            _jsonWebTokenConfiguration = jsonWebTokenConfiguration;
        }


        private IActionResult GetActionResaultFromUserAuthentication(UserAuthenticationDTO userAuthentication)
        {
            if (userAuthentication == null) return BadRequest("User authentication can not be null.");
            if (userAuthentication.Salt == null) return BadRequest("Salt can not be null");
            if (userAuthentication.UserName == null) return BadRequest("User name can not be null");
            if (userAuthentication.HashPassword == null) return BadRequest("Hash password can not be null");
            return Ok();
        }


        private string CreateJsonWebTokenWithUser(User user)
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
                new Claim("UserId", user.UserId.ToString()),
                new Claim("UserName", user.UserName),
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
        public async Task<IActionResult> Register([FromBody] UserAuthenticationDTO userAuthentication)
        {
            var actionResault = GetActionResaultFromUserAuthentication(userAuthentication);

            if (actionResault is OkResult)
            {
                var userName = userAuthentication.UserName;
                var user = await _userRepository.GetUserWithNameAsync(userName);

                if (user != null)
                {
                    return BadRequest("Username is taken");
                }

                user = _userConverter.ConvertFromUserAuthentication(userAuthentication);
                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();


                var jsonWebToken = CreateJsonWebTokenWithUser(user);
                return Ok(jsonWebToken);
            }

            return actionResault;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserAuthenticationDTO userAuthentication)
        {
            var actionResault = GetActionResaultFromUserAuthentication(userAuthentication);
            var userName = userAuthentication.UserName;
            var user = await _userRepository.GetUserWithNameAsync(userName);


            if (user == null)
            {
                return BadRequest("Invalid username or password");
            }

            if (user.HashPassword != userAuthentication.HashPassword)
            {
                return BadRequest("Invalid username or password");
            }

            if (actionResault is BadRequestResult)
            {
                return actionResault;
            }

            var jsonWebToken = CreateJsonWebTokenWithUser(user);
            return Ok(jsonWebToken);
        }



    }
}
