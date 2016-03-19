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

        public User GetUserByEmail(string email)
        {
            return dbContext.CustomUsers.FirstOrDefault(user => user.Email.Equals(email));
        }

        public User GetUserById(int userId)
        {
            return dbContext.CustomUsers.FirstOrDefault(user => user.Id == userId);
        }

        public IEnumerable<User> GetUsers()
        {
            return dbContext.CustomUsers;
        }

        internal void CreateResetDigest(User user)
        {
            user.CreateResetDigest();
            dbContext.SaveChanges();
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

        public bool UpdateUser(User user)
        {
            try
            {
                var userInDB = dbContext.CustomUsers.Find(user.Id);

                dbContext.Entry(userInDB).CurrentValues.SetValues(user);
                dbContext.Entry(userInDB).State = EntityState.Modified;                
                if (user.Password == null)
                {
                    dbContext.Entry(userInDB).Property("PasswordDigest").IsModified = false;
                }
                // dbContext.Entry(userInDB).Property("Admin").IsModified = false;

                dbContext.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        internal void DeleteUser(int id)
        {
            var userToBeDeleted = dbContext.CustomUsers.SingleOrDefault(user => user.Id == id);

            if (userToBeDeleted != null)
            {
                dbContext.CustomUsers.Remove(userToBeDeleted);
                dbContext.SaveChanges();
            }
        }

        internal bool UpdatePassword(User user, string password, string passwordConfirmation)
        {
            try
            {
                user.Password = password;
                user.PasswordConfirmation = passwordConfirmation;
                dbContext.Entry(user).Property("PasswordDigest").IsModified = true;
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