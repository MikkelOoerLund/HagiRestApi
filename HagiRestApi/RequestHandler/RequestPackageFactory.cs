namespace HagiRestApi
{
    public class RequestPackageFactory
    {
        private Dictionary<RequestType, RequestPackage> _requestPackageByType;

        public RequestPackageFactory()
        {
            _requestPackageByType = new Dictionary<RequestType, RequestPackage>();
        }

        public RequestPackage GetRequestPackage(RequestType requestType, byte[] dataInBytes)
        {
            if (_requestPackageByType.ContainsKey(requestType))
            {
                var requestPackage = _requestPackageByType[requestType];
                requestPackage.DataInBytes = dataInBytes;
                return requestPackage;
            }

            var newRequestPackage = new RequestPackage()
            {
                RequestType = requestType,
                DataInBytes = dataInBytes,
            };

            _requestPackageByType[requestType] = newRequestPackage;
            return newRequestPackage;
        }


    }
}
