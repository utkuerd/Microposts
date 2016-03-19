using Micropost.DataAccess;
using Micropost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;

namespace Micropost.Helper
{
    public class SessionsHelper
    {        
        private static User _currentUser;
        private static UserRepository userRepository = new UserRepository(new ApplicationDbContext());

        internal static void LogIn(User loginUser)
        {
            HttpContext context = HttpContext.Current;
            context.Session["UserId"] = loginUser.Id;
        }

        public static User CurrentUser()
        {
            HttpContext context = HttpContext.Current;
            try
            {
                if (context.Session["UserId"] != null)
                {
                    _currentUser = _currentUser ?? userRepository.GetUserById(Convert.ToInt32(context.Session["UserId"]));
                }
                else
                {
                    var cookies = HttpContext.Current.Request.Cookies;

                    var sessionCookie = cookies["RubyTutorial"];
                    if (sessionCookie == null) return null;

                    var userIdCookieVal = sessionCookie["UserId"];

                    int userId = Convert.ToInt32(
                                    Encoding.UTF8.GetString(
                                        MachineKey.Unprotect(
                                            Convert.FromBase64String(userIdCookieVal))));

                    User authUser = userRepository.GetUserById(userId);
                    if (authUser != null && authUser.TokenAuthenticated("Remember",sessionCookie["RememberToken"]))
                    {
                        LogIn(authUser);
                        _currentUser = authUser;
                    }
                }
            }
            catch (FormatException)
            { }
            catch (CryptographicException)
            { }
            return _currentUser;
        }

        internal static void Remember(User loginUser)
        {
            loginUser.Remember();

            var cookies = HttpContext.Current.Response.Cookies;
            HttpCookie sessionCookie = new HttpCookie("RubyTutorial");

            var UserIdCookieVal = Convert.ToBase64String(
                                    MachineKey.Protect(
                                        Encoding.UTF8.GetBytes(loginUser.Id.ToString()), "an authentication token"));            
            sessionCookie.Values.Add("UserId", UserIdCookieVal );
            sessionCookie.Values.Add("RememberToken", loginUser.RememberToken);
            sessionCookie.Expires = DateTime.Now.AddYears(20);

            cookies.Add(sessionCookie);
            
        }

        public static bool IsLoggedIn()
        {
            return CurrentUser() != null;
        }

        public static bool IsCurrentUser(User user)
        {
            return CurrentUser() != user;
        }

        internal static void LogOut()
        {
            Forget(_currentUser);

            HttpContext context = HttpContext.Current;
            context.Session.Remove("UserId");

            _currentUser = null;
        }

        internal static void Forget(User user)
        {
            user.Forget();

            var cookies = HttpContext.Current.Request.Cookies;

            var sessionCookie = cookies["RubyTutorial"];
            if (sessionCookie == null) return;

            sessionCookie.Expires = DateTime.Now.AddDays(-10);
            sessionCookie.Value = null;

            HttpContext.Current.Response.SetCookie(sessionCookie);
        }
    }
}