using HagiDatabaseDomain;
using MediatR;

namespace HagiRestApi.Controllers
{
    public class GetAllUsersRequest : IRequest<List<User>>
    {
    }
}
