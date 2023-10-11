using HagiDatabaseDomain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HagiRestApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var pokemonSpecies = await _userRepository.GetAllAsync();
            return Ok(pokemonSpecies);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var pokemonSpecies = await _userRepository.GetAsync(id);

            if (pokemonSpecies == null)
            {
                return NotFound();
            }

            return Ok(pokemonSpecies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser()
        {
            var user = await ReadUserFromStream();
         
            if (user == null)
            {
                return BadRequest("The request body cannot be null.");
            }

            user.UserId = 0;


            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();


            var routeValues = new { id = user.UserId };
            return CreatedAtRoute("GetUser", routeValues, user);
        }

        private async Task<User> ReadUserFromStream()
        {
            string requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
            return JsonConvert.DeserializeObject<User>(requestBody);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id)
        {
            var user = await ReadUserFromStream();


            if (user == null)
            {
                return BadRequest("User cannot be null.");
            }

            var userId = user.UserId;

            if (userId != id)
            {
                return BadRequest($"User with UserId: {userId}, dosent match the given id: {id}");
            }

            var existingPokemonSpecies = await _userRepository.GetAsync(id);

            if (existingPokemonSpecies == null)
            {
                return NotFound();
            }

            _userRepository.SetValues(existingPokemonSpecies, user);
            await _userRepository.SaveChangesAsync();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePokemonSpecies(int id)
        {
            var pokemonSpeciesToDelete = await _userRepository.GetAsync(id);

            if (pokemonSpeciesToDelete == null)
            {
                return NotFound();
            }

            _userRepository.Remove(pokemonSpeciesToDelete);
            await _userRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
