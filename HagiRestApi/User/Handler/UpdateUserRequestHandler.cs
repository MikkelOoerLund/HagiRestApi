using AutoMapper;
using HagiDatabaseDomain;
using MediatR;

namespace HagiRestApi.Controllers
{
    public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest, User>
    {
        private readonly IMapper _mapper;
        private readonly UserRepository _userRepository;

        public UpdateUserRequestHandler(IMapper mapper, UserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }


        public async Task<User> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var userAuthentication = request.UserAuthenticationDTO;
            userAuthentication.UserId = userId;

            var userValueContainer = _mapper.Map<User>(userAuthentication);
            var userToUpdate = await _userRepository.GetAsync(userId);

            _userRepository.SetValues(userToUpdate, userValueContainer);
            await _userRepository.SaveChangesAsync();
            return userToUpdate;
        }
    }
}
