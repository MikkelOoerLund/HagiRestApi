using AutoMapper;
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
        private IMapper _mapper;
        private UserRepository _userRepository;
        private JsonWebTokenConfiguration _jsonWebTokenConfiguration;

        public AuthenticationController(JsonWebTokenConfiguration jsonWebTokenConfiguration, UserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _jsonWebTokenConfiguration = jsonWebTokenConfiguration;
        }


        private IActionResult GetActionResaultFromUserAuthentication(UserAuthenticationDTO userAuthenticationDTO)
        {
            if (userAuthenticationDTO == null) return BadRequest("User authentication can not be null.");
            if (userAuthenticationDTO.Salt == null) return BadRequest("Salt can not be null");
            if (userAuthenticationDTO.UserName == null) return BadRequest("User name can not be null");
            if (userAuthenticationDTO.HashPassword == null) return BadRequest("Hash password can not be null");

            return Ok();
        }


        private string CreateJsonWebTokenFromUser(User user)
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

        [HttpGet("{name:alpha}")]
        public async Task<IActionResult> GetSalt(string name)
        {
            var user = await _userRepository.GetUserWithNameAsync(name);

            if (user == null)
            {
                return BadRequest("Invalid username or password");
            }


            return Ok(user.Salt);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserAuthenticationDTO userAuthenticationDTO)
        {
            var actionResault = GetActionResaultFromUserAuthentication(userAuthenticationDTO);

            if (actionResault is OkResult)
            {
                var user = _mapper.Map<User>(userAuthenticationDTO);

                var existingUser = await _userRepository.GetUserWithNameAsync(user.UserName);

                if (existingUser == null)
                {
                    var jsonWebToken = CreateJsonWebTokenFromUser(user);

                    await _userRepository.AddAsync(user);
                    await _userRepository.SaveChangesAsync();
                    return Ok(jsonWebToken);
                }

                return BadRequest("Username is taken");
            }

            return actionResault;
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login(UserAuthenticationDTO userAuthenticationDTO)
        {
            var mappedUser = _mapper.Map<User>(userAuthenticationDTO);
            var user = await _userRepository.GetUserWithNameAsync(mappedUser.UserName);

            if (user == null)
            {
                return BadRequest("Invalid username or password");
            }


            if (user.HashPassword != mappedUser.HashPassword)
            {
                return BadRequest("Invalid username or password");
            }

            var jsonWebToken = CreateJsonWebTokenFromUser(user);
            return Ok(jsonWebToken);
        }



    }
}
