using MediatR;

namespace HagiRestApi.Controllers
{
    public class DeleteUserWithIdRequest : IRequest
    {
        public int UserId { get; set; }
    }
}
