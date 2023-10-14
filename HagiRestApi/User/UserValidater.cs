using HagiDatabaseDomain;

namespace HagiRestApi
{
    public class UserValidater
    {
        private UserRepository _userRepository;

        public UserValidater(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> IsUserNameTakenAsync(string userName)
        {
            return  await _userRepository.HasUserWithNameAsync(userName);
        }
    }
}
