namespace HagiRestApi
{
    public interface IRequestHandler
    {
        public IActionResult HandleRequest(RequestPackage requestPackage);
    }
}
