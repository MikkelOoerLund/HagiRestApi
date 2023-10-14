using AutoMapper;
using HagiDatabaseDomain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HagiRestApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private IMapper _mapper;
        private RequestPackageFactory _requestPackageFactory;
        private RequestHandlerChainCollection _requestHandlerChainCollection;
        //private UserValidater _userValidater;
        //private UserConverter _userConverter;
        //private UserRepository _userRepository;

        public UserController(IMapper mapper, RequestPackageFactory requestPackageFactory, RequestHandlerChainCollection requestHandlerChainCollection/* UserValidater userValidater, UserRepository userRepository, UserConverter userConverter*/)
        {
            _mapper = mapper;
            //_userConverter = userConverter;
            //_userRepository = userRepository;
            _requestPackageFactory = requestPackageFactory;
            _requestHandlerChainCollection = requestHandlerChainCollection;
        }

        private RequestHandlerChain GetRequestHandlerChain(RequestType requestType)
        {
            return _requestHandlerChainCollection.GetRequestHandlerChain(requestType);
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var requestType = RequestType.GetAll;
            var requestPackage = _requestPackageFactory.GetRequestPackage(requestType, null);
            var requestHandlerChain = GetRequestHandlerChain(RequestType.GetWithId);
            return requestHandlerChain.HandleRequest(requestPackage) as IActionResult;

            //var users = await _userRepository.GetAllAsync();
            //return Ok(users);
        }

        //[HttpGet("{id:int}", Name = "GetUserWithId")]
        //public async Task<IActionResult> GetUserWithId(int id)
        //{
        //    var user = await _userRepository.GetAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(user);
        //}

        //[HttpGet("{name:regex(.*)}", Name = "GetUserWithName")]
        //public async Task<IActionResult> GetUserWithName(string name)
        //{
        //    var userWithName = await _userRepository.GetUserWithNameAsync(name);

        //    if (userWithName == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(userWithName);
        //}



        //[HttpPost]
        //public async Task<IActionResult> CreateUser([FromBody] UserAuthenticationDTO userAuthenticationDTO)
        //{


        //    var userName = userAuthenticationDTO.UserName;

        //    var isUserNameTaken = await _userValidater.IsUserNameTakenAsync(userName);

        //    if (isUserNameTaken)
        //    {
        //        return BadRequest("User name is takne");
        //    }


        //    var user = _mapper.Map<User>(userAuthenticationDTO);

        //    await _userRepository.AddAsync(user);
        //    await _userRepository.SaveChangesAsync();

        //    var routeValues = new { id = user.UserId };
        //    return CreatedAtRoute("GetUserWithId", routeValues, user);
        //}


        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateUser(int id, [FromBody] UserAuthenticationDTO userLogin)
        //{
        //    if (userLogin == null)
        //    {
        //        return BadRequest("User cannot be null.");
        //    }

        //    var user = _userConverter.ConvertFromUserAuthentication(userLogin);

        //    var existingUser = await _userRepository.GetAsync(id);

        //    if (existingUser == null)
        //    {
        //        return NotFound();
        //    }

        //    user.UserId = id;

        //    _userRepository.SetValues(existingUser, user);
        //    await _userRepository.SaveChangesAsync();

        //    return Ok(user);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var userToDelete = await _userRepository.GetAsync(id);

        //    if (userToDelete == null)
        //    {
        //        return NotFound();
        //    }

        //    _userRepository.Remove(userToDelete);
        //    await _userRepository.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}
