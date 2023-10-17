using HagiDatabaseDomain;
using MediatR;

namespace HagiRestApi.Controllers
{
    public class CreateUserRequest : IRequest<User>
    {
        public UserAuthenticationDTO UserAuthenticationDTO { get; set; }
    }
}
