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
        bool SaveUser(User obj);
    }
}