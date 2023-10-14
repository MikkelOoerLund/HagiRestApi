using Microsoft.AspNetCore.Mvc;

namespace HagiRestApi
{
    public interface IAsyncRequestHandler
    {
        public Task<IActionResult> HandleRequestAsync(RequestPackage requestPackage);
    }
}
