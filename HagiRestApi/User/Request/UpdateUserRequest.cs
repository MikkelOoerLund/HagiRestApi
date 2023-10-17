using HagiDatabaseDomain;
using MediatR;

namespace HagiRestApi.Controllers
{
    public class UpdateUserRequest : IRequest<User>
    {
        public int UserId { get; set; }
        public UserAuthenticationDTO UserAuthenticationDTO { get; set; }
    }
}
