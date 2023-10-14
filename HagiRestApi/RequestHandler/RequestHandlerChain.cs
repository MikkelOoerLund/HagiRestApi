using Microsoft.AspNetCore.Mvc;

namespace HagiRestApi
{
    public class RequestHandlerChain
    {
        private List<IRequestHandler> _requestHandlers;

        public RequestHandlerChain()
        {
            _requestHandlers = new List<IRequestHandler>();
        }

        public IActionResult HandleRequest(RequestPackage request)
        {
            foreach (var requestHandler in _requestHandlers)
            {
                var actionResult = requestHandler.HandleRequest(request);
                if (actionResult == null) continue;
                return actionResult;
            }

            throw new Exception("Request was not handled");
        }
    }
}
