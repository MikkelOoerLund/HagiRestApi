using HagiDatabaseDomain;
using MediatR;

namespace HagiRestApi.Controllers
{
    public class GetUserWithIdRequest : IRequest<User>
    {
        public int UserId { get; set; }
    }
}
