using AutoMapper;
using HagiDatabaseDomain;

namespace HagiRestApi
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAuthenticationDTO, User>();
        }
    }
}
