using HagiDatabaseDomain;
using MediatR;

namespace HagiRestApi.Controllers
{
    public class GetUserWithNameRequest : IRequest<User>
    {
        public string UserName { get; set; }
    }
}
