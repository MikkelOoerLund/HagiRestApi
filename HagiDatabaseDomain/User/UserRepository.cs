using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HagiDatabaseDomain
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(UserContext userContext) : base(userContext)
        {
        }

        public async Task<User> GetUserWithNameAsync(string name)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.UserName == name);
        }


        public async Task<bool> HasUserWithNameAsync(string name)
        {
            return await DbSet.AnyAsync(x => x.UserName == name);
        }


    }
}
