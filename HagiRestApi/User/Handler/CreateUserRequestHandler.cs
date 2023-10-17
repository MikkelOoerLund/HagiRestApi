using AutoMapper;
using HagiDatabaseDomain;
using MediatR;

namespace HagiRestApi.Controllers
{
    public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, User>
    {
        private readonly IMapper _mapper;
        private readonly UserRepository _userRepository;

        public CreateUserRequestHandler(IMapper mapper, UserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<User> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var userAuthentication = request.UserAuthenticationDTO;
            var user = _mapper.Map<User>(userAuthentication);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }
    }
}
