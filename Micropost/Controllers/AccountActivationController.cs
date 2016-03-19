using Micropost.DataAccess;
using Micropost.Helper;
using Micropost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Micropost.Controllers
{
    public class AccountActivationController : Controller
    {
        private UserRepository userRepository;

        public AccountActivationController()
        {
            this.userRepository = new UserRepository(new ApplicationDbContext());
        }

        public ActionResult Edit(string Token, string Email)
        {
            User user = userRepository.GetUserByEmail(Email);
            if (user != null && ! user.Activated && user.TokenAuthenticated("Activation", Token))
            {
                user.Activate();
                SessionsHelper.LogIn(user);
                TempData["success"] = "Account activated";

                return RedirectToRoute("UserPath", new { id = user.Id });
            }
            else
            {
                TempData["danger"] = "Invalid activation link";
                return RedirectToRoute("Default");
            }            
        }
    }
}