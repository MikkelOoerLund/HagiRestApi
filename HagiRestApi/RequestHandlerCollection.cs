using Microsoft.AspNetCore.Mvc;

namespace HagiRestApi
{
    public class RequestHandlerCollection<TRequest>
    {
        private List<IRequestHandler<TRequest>> _requestHandlers;

        public RequestHandlerCollection()
        {
            _requestHandlers = new List<IRequestHandler<TRequest>>();
        }


        public void AddRequestHandler(IRequestHandler<TRequest> requestHandler)
        {
            if (_requestHandlers.Contains(requestHandler)) return;
            _requestHandlers.Add(requestHandler);
        }


        public void AddRequestHandlers(IEnumerable<IRequestHandler<TRequest>> requestHandlers)
        {
            foreach (var requestHandler in requestHandlers)
            {
                if (_requestHandlers.Contains(requestHandler))
                {
                    continue;
                }

                _requestHandlers.Add(requestHandler);
            }
        }


        public IActionResult HandleRequest(TRequest request)
        {
            foreach (var requestHandler in _requestHandlers)
            {
                var response = requestHandler.HandleRequest(request);
                if (response == null) continue;
            }

            return null;
        }
    }
}
