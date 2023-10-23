using HagiDatabaseDomain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace HagiRestApi.Controllers
{



    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private IMediator _mediator;


        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var getAllUsersRequest = new GetAllUsersRequest();
            var users = await _mediator.Send(getAllUsersRequest);
            return Ok(users);
        }

        [HttpGet("{id:int}", Name = "GetUserWithId")]
        public async Task<IActionResult> GetUserWithId(int id)
        {
            var getUserWithIdRequest = new GetUserWithIdRequest()
            {
                UserId = id,
            };

            var user = await _mediator.Send(getUserWithIdRequest);
            return Ok(user);
        }

        [HttpGet("{name:alpha}", Name = "GetUserWithName")]
        public async Task<IActionResult> GetUserWithName(string name)
        {
            var getUserWithNameRequest = new GetUserWithNameRequest()
            {
                UserName = name,
            };

            var user = await _mediator.Send(getUserWithNameRequest);
            if (user == null) return BadRequest($"No user has the given user name: {name}");
            return Ok(user);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserAuthenticationDTO userAuthenticationDTO)
        {
            var request = new CreateUserRequest()
            {
                UserAuthenticationDTO = userAuthenticationDTO,
            };

            var user = await _mediator.Send(request);
            var routeValues = new { id = user.UserId };
            return CreatedAtRoute("GetUserWithId", routeValues, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserAuthenticationDTO userAuthenticationDTO)
        {
            var request = new UpdateUserRequest()
            {
                UserId = id,
                UserAuthenticationDTO = userAuthenticationDTO,
            };


            var user = await _mediator.Send(request);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var request = new DeleteUserWithIdRequest()
            {
                UserId = id,
            };

            await _mediator.Send(request);

            return NoContent();
        }
    }
}
