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
    public class PasswordResetsController : Controller
    {
        private UserRepository userRepository;        

        public PasswordResetsController()
        {
            this.userRepository = new UserRepository(new ApplicationDbContext());
        }


        public ActionResult New()
        {
            return View();
        }

        public ActionResult Create(Dictionary<string,string> password_reset)
        {
            User user = userRepository.GetUserByEmail(password_reset["Email"]);

            if (user != null)
            {
                userRepository.CreateResetDigest(user);                                
                user.SendPasswordResetEmail();
                TempData["info"] = "Email sent with password reset instructions";
                return RedirectToRoute("Default");
            }
            else
            {
                TempData["danger"] = "Email address not found";
                return View("New");
            }

        }

        public ActionResult Edit(string email, string token)
        {
            User user = GetUser(email);
                        
            var redirect = ValidUser(user, token) ?? CheckExpiration(user);
            if (redirect != null)
            {                
                return redirect;
            }

            return View(user);
        }

        public ActionResult Update([Bind(Include = "Password,PasswordConfirmation,Email")]User user, String token)
        {
            User updateUser = GetUser(user.Email);

            var redirect = ValidUser(updateUser, token) ?? CheckExpiration(updateUser);
            if (redirect != null)
            {
                return redirect;
            }

            if (user.Password == null || user.Password == "")
            {
                ViewData.ModelState.AddModelError("Password", "Password can't be empty");
            }
            else if (userRepository.UpdatePassword(updateUser, user.Password, user.PasswordConfirmation))
            {
                SessionsHelper.LogIn(updateUser);
                TempData["success"] = "Password has been reset";
                return RedirectToRoute("User", new { id = updateUser.Id, action = "Index" });
            }
            ModelState.Remove("Name");
            return View("Edit",user);
        }

        private ActionResult ValidUser(User user, string id)
        {
            if (user == null ||  !user.Activated || !user.TokenAuthenticated("Reset",id))
            {
                return RedirectToRoute("Default");
            }
            else
            {
                return null;
            }
        }

        private User GetUser(string email)
        {
            return userRepository.GetUserByEmail(email);
        }

        private ActionResult CheckExpiration(User user)
        {
            if (user.PasswordResetExpired())
            {
                TempData["danger"] = "Password reset has expired";
                return RedirectToRoute("PasswordResetRoute", new { action = "New" });
            }
            else
            {
                return null;
            }
        }
    }
}