using HagiDatabaseDomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace HagiRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private UserConverter _userConverter;
        private UserRepository _userRepository;
        private JsonWebTokenFactory _jsonWebTokenFactory;

        public AuthenticationController(UserConverter userConverter, UserRepository userRepository, JsonWebTokenFactory jsonWebTokenFactory)
        {
            _userConverter = userConverter;
            _userRepository = userRepository;
            _jsonWebTokenFactory = jsonWebTokenFactory;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserAuthenticationDTO userAuthentication)
        {
            if (userAuthentication == null)
            {
                return BadRequest("UserAuthentication can not be null");
            }

            var userName = userAuthentication.UserName;
            var isUserNameTaken = await _userRepository.HasUserWithNameAsync(userName);

            if (isUserNameTaken)
            {
                return BadRequest("Username is taken");
            }

            var user = _userConverter.ConvertToUser(userAuthentication);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return Ok(user);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(UserAuthenticationDTO userAuthentication)
        {
            var userName = userAuthentication.UserName;
            var user = await _userRepository.GetUserWithNameAsync(userName);


            if (user == null)
            {
                // Debugging
                return BadRequest("Username is invalid");

                //return BadRequest("Username or password is invalid");
            }


            if (user.HashPassword != userAuthentication.HashPassword)
            {
                // Debugging
                return BadRequest("Password is invalid");

                //return BadRequest("Username or password is invalid");
            }



            var token = _jsonWebTokenFactory.CreateJsonWebToken(user);

            var tokenContainer = new JsonWebTokenDTO()
            {
                Token = token,
            };

            return Ok(tokenContainer);
        }



    }
}
