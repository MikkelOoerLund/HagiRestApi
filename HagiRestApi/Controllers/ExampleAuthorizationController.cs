using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HagiRestApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ExampleAuthorizationController : Controller
    {
        private string _secretData;

        public ExampleAuthorizationController()
        {
            _secretData = "Example secret data";
        }



        [HttpGet]
        public IActionResult GetSecretData()
        {
            return Ok(_secretData);
        }



    }
}
