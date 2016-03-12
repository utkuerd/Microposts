using Micropost.DataAccess;
using Micropost.Helper;
using Micropost.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Micropost.Controllers
{
    public class UsersController : Controller
    {
        private UserRepository userRepository;

        public UsersController()
        {
            this.userRepository = new UserRepository(new ApplicationDbContext());
        }

        // GET: User/new
        public ActionResult New()
        {
            return View(new User());
        }

        // GET: User
        
        public ActionResult Index(int? page)
        {
            int pageSize = 25;

            var users = userRepository.GetUsers().OrderBy(user => user.Id);
            return View(users.ToPagedList(page ?? 1,pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User newUser)
        {        
            if (ModelState.IsValid && userRepository.SaveUser(newUser))
            {
                SessionsHelper.LogIn(newUser);
                TempData["success"] = "Welcome to the Sample App!";
                return RedirectToRoute("UserPath", new { id = newUser.Id });            
            }
            else
            {
                return View("New", newUser);
            }
        }

        public ActionResult Show(int id)
        {
            var user = userRepository.GetUserById(id);
            return View(user);
        }

        public ActionResult Edit(int id)
        {
            var redirect = LoggedInUser() ?? CorrectUser(id);
            if (redirect != null)
            {
                StoreLocation();
                return redirect;
            }

            var user = userRepository.GetUserById(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include="Name,Email,Password,PasswordConfirmation")] User user)
        {
            var redirect = LoggedInUser() ?? CorrectUser(user.Id);
            if (redirect != null)
            {
                StoreLocation();
                return redirect;
            }
            
            if (ModelState.IsValid && userRepository.UpdateUser(user))
            {
                TempData["success"] = "Profile updated.";
                return RedirectToRoute("UserPath", new { id = user.Id, action= "Show" });
            }
            else
            {
                return View("Edit", user);
            }
        }

        public ActionResult Destroy(int Id)
        {
            var redirect = LoggedInUser();
            if (redirect != null)
            {
                StoreLocation();
                return redirect;
            }

            userRepository.DeleteUser(id);
            TempData["success"] = "User Deleted";

            return RedirectToRoute("AllUsersPath");
        }

        private ActionResult LoggedInUser()
        {
            if (!SessionsHelper.IsLoggedIn())
            {
                TempData["danger"] = "Please log in";
                return RedirectToRoute("LoginPathGet");
            }
            else return null;
        }

        private ActionResult CorrectUser(int id)
        {
            User u = userRepository.GetUserById(id);
            if (! SessionsHelper.IsCurrentUser(u))
            {
                return RedirectToRoute("LoginPathGet");
            }
            else
            {
                return null;
            }
        }

        private ActionResult AdminUser()
        {
            if (! SessionsHelper.CurrentUser().Admin)
            {
                return RedirectToRoute("Default");
            }
            else
            {
                return null;
            }
        }

        private void StoreLocation()
        {
            if (Request.RequestType.Equals("GET"))
            {
                Session["ForwardingUrl"] = Request.Url;
            }
        }
    }
}