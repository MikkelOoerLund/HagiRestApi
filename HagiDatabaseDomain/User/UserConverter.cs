namespace HagiDatabaseDomain
{
    public class UserConverter
    {
        public User ConvertFromUserAuthentication(UserAuthenticationDTO userAuthentication)
        {
            return new User()
            {
                Salt = userAuthentication.Salt,
                UserId = userAuthentication.UserId,
                UserName = userAuthentication.UserName,
                HashPassword = userAuthentication.HashPassword,
            };
        }


        public UserAuthenticationDTO ConvertToUserAuthentication(User user)
        {
            return new UserAuthenticationDTO()
            {
                Salt = user.Salt,
                UserId = user.UserId,
                UserName = user.UserName,
                HashPassword = user.HashPassword,
            };
        }
    }
}