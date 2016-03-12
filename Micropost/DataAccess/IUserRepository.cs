using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Micropost.Models;

namespace Micropost.DataAccess
{
    public interface IUserRepository
    {
        User GetUserById(int userId);
        User GetUserByEmail(string email);
        bool SaveUser(User obj);
        bool UpdateUser(User user);
        IEnumerable<User> GetUsers();
    }
}