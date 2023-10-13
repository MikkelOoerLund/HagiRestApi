namespace HagiDatabaseDomain
{
    public class UserConverter
    {
        public User ConvertFromUserAuthentication(UserAuthenticationDTO userAuthentication)
        {
            return new User()
            {
                Salt = userAuthentication.Salt,
                UserName = userAuthentication.UserName,
                HashPassword = userAuthentication.HashPassword,
            };
        }


        public UserAuthenticationDTO ConvertToUserAuthentication(User user)
        {
            return new UserAuthenticationDTO()
            {
                Salt = user.Salt,
                UserName = user.UserName,
                HashPassword = user.HashPassword,
            };
        }
    }
}