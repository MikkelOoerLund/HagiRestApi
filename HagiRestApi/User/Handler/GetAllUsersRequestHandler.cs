using HagiDatabaseDomain;
using MediatR;

namespace HagiRestApi.Controllers
{
    public class GetAllUsersRequestHandler : IRequestHandler<GetAllUsersRequest, List<User>>
    {
        private readonly UserRepository _userRepository;

        public GetAllUsersRequestHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            return users;
        }
    }
}
