using Azure.Core;
using Microsoft.AspNetCore.Mvc;

namespace HagiRestApi
{

    public interface IRequestHandler<TRequest>
    {
        public IActionResult HandleRequest(TRequest request);
    }
}
