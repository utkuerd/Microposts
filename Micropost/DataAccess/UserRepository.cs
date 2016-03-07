using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Micropost.Models;

namespace Micropost.DataAccess
{   
    public class UserRepository : IUserRepository 
    {
        private ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public User GetUserById(int userId)
        {
            return dbContext.CustomUsers.First(user => user.Id == userId);
        }

        public bool SaveUser(User newUser)
        {
            try
            {
                dbContext.CustomUsers.Add(newUser);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}