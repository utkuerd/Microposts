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
    public class SessionsController : Controller
    {
        private UserRepository userRepository;        

        public SessionsController()
        {
            this.userRepository = new UserRepository(new ApplicationDbContext());
        }

        public ActionResult New()
        {
            var session = new Dictionary<string, string>();
            return View(session);
        }

        public ActionResult Create(Dictionary<string,string> session)
        {
            User loginUser = userRepository.GetUserByEmail(session["Email"].ToLowerInvariant()); 
            if (loginUser != null && loginUser.Authenticate(session["Password"]))
            {
                SessionsHelper.LogIn(loginUser);
                if (Convert.ToBoolean(session["RememberMe"]))
                {
                    SessionsHelper.Remember(loginUser);
                }
                else
                {
                    SessionsHelper.Forget(loginUser);
                }                                   
                return RedirectBackOr("UserPath", new { id = loginUser.Id });
            }
            else
            {
                TempData["danger"] = "Invalid email/password combination";
                return View("New", session);
            }
        }


        public ActionResult Destroy()
        {
            if (SessionsHelper.IsLoggedIn())
            {
                SessionsHelper.LogOut();
            }
            return RedirectToRoute("Default");
        }

        private ActionResult RedirectBackOr(String spec, object p)
        {
            Uri url = null;
            try
            {
                url = Session["ForwardingUrl"] as Uri;
                Session.Remove("ForwardingUrl");
                return Redirect(url.ToString());
            }
            catch (Exception)
            {                
                return RedirectToRoute(spec, p);
            }
        }
    }
}