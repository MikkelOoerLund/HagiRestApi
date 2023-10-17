using HagiDatabaseDomain;
using MediatR;

namespace HagiRestApi.Controllers
{
    public class GetUserWithNameRequestHandler : IRequestHandler<GetUserWithNameRequest, User>
    {
        private readonly UserRepository _userRepository;

        public GetUserWithNameRequestHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(GetUserWithNameRequest request, CancellationToken cancellationToken)
        {
            var userName = request.UserName;
            var user = await _userRepository.GetUserWithNameAsync(userName);
            return user;
        }
    }
}
