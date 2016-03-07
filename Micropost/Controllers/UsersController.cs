using Micropost.DataAccess;
using Micropost.Models;
using System;
using System.Collections.Generic;
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
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User newUser)
        {        
            if (ModelState.IsValid && userRepository.SaveUser(newUser))
            {
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
    }
}