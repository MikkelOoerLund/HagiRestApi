using HagiDatabaseDomain;
using MediatR;

namespace HagiRestApi.Controllers
{
    public class GetUserWithIdRequestHandler : IRequestHandler<GetUserWithIdRequest, User>
    {
        private readonly UserRepository _userRepository;

        public GetUserWithIdRequestHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(GetUserWithIdRequest request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;

            var user = await _userRepository.GetAsync(userId);
            return user;
        }
    }
}
