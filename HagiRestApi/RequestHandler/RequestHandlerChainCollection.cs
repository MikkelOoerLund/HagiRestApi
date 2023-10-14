namespace HagiRestApi
{
    public class RequestHandlerChainCollection
    {
        private Dictionary<RequestType, RequestHandlerChain> _requestChainByType;

        public RequestHandlerChainCollection()
        {
            _requestChainByType = new Dictionary<RequestType, RequestHandlerChain>();
        }

        public void AddRequestHandlerChain(RequestType requestType, RequestHandlerChain requestChain)
        {
            _requestChainByType.Add(requestType, requestChain);
        }

        public RequestHandlerChain GetRequestHandlerChain(RequestType requestType)
        {
            return _requestChainByType[requestType];
        }
    }
}
