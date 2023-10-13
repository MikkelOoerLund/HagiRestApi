using HagiDatabaseDomain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HagiRestApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private UserConverter _userConverter;
        private UserRepository _userRepository;

        public UserController(UserRepository userRepository, UserConverter userConverter)
        {
            _userConverter = userConverter;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}", Name = "GetUserWithId")]
        public async Task<IActionResult> GetUserWithId(int id)
        {
            var user = await _userRepository.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("{name:alpha}", Name = "GetUserWithName")]
        public async Task<IActionResult> GetUserWithName(string name)
        {
            var userWithName = await _userRepository.GetUserWithNameAsync(name);

            if (userWithName == null)
            {
                return NotFound();
            }

            return Ok(userWithName);
        }



        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserAuthenticationDTO userAuthentication)
        {
            if (userAuthentication == null)
            {
                return BadRequest("The request body cannot be null.");
            }

            var existingUser = await _userRepository.GetUserWithNameAsync(userAuthentication.UserName);

            if (existingUser != null)
            {
                return BadRequest($"User with UserName: {userAuthentication.UserName} already exists");
            }


            var user = _userConverter.ConvertFromUserAuthentication(userAuthentication);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var routeValues = new { id = user.UserId };
            return CreatedAtRoute("GetUserWithId", routeValues, user);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserAuthenticationDTO userLogin)
        {
            if (userLogin == null)
            {
                return BadRequest("User cannot be null.");
            }

            var user = _userConverter.ConvertFromUserAuthentication(userLogin);

            var existingUser = await _userRepository.GetAsync(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            user.UserId = id;

            _userRepository.SetValues(existingUser, user);
            await _userRepository.SaveChangesAsync();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userToDelete = await _userRepository.GetAsync(id);

            if (userToDelete == null)
            {
                return NotFound();
            }

            _userRepository.Remove(userToDelete);
            await _userRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
