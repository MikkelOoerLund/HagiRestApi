using HagiDatabaseDomain;
using MediatR;

namespace HagiRestApi.Controllers
{
    public class DeleteUserWithIdRequestHandler : IRequestHandler<DeleteUserWithIdRequest>
    {
        private readonly UserRepository _userRepository;

        public DeleteUserWithIdRequestHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(DeleteUserWithIdRequest request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var userToDelete = await _userRepository.GetAsync(userId);

            _userRepository.Remove(userToDelete);
            await _userRepository.SaveChangesAsync();
        }
    }
}
