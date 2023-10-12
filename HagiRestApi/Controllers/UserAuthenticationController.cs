using HagiDatabaseDomain;
using Microsoft.AspNetCore.Mvc;

namespace HagiRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAuthenticationController : Controller
    {
        private UserRepository _userRepository;

        public UserAuthenticationController(UserRepository userRepository)
        {

        }


    }
}
