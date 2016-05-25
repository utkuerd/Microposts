using Microposts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microposts.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool Following(ApplicationUser user1, ApplicationUser user2)
        {
            return context.Users.Find(user1.Id).Following.Contains(user2);
        }
    }

    public interface IUserRepository
    {
        bool Following(ApplicationUser user1, ApplicationUser user2);

    }
}