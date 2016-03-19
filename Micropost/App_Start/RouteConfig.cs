using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Micropost
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Default",
            url: "",
            defaults: new { controller = "StaticPages", action = "Home", id = UrlParameter.Optional }
        );

            routes.MapRoute(
                name: "Help",
                url: "help",
                defaults: new { controller = "StaticPages", action = "Help" }
            );
            routes.MapRoute(
                name: "About",
                url: "about",
                defaults: new { controller = "StaticPages", action = "About" }
            );
            routes.MapRoute(
                name: "Contact",
                url: "contact",
                defaults: new { controller = "StaticPages", action = "Contact" }
            );
            routes.MapRoute(
                name: "SignUp",
                url: "signup",
                defaults: new { controller = "Users", action = "New" }
            );

            routes.MapRoute(
                name: "UsersPath",
                url: "users",
                defaults: new { controller = "Users", action = "Create" },
                constraints: new { httpmethod = new HttpMethodConstraint("POST") }
            );

            routes.MapRoute(
                name: "UserPath",
                url: "users/{id}",
                defaults: new { controller = "Users", action = "Show" },
                constraints: new { httpmethod = new HttpMethodConstraint("GET") }
            );

            routes.MapRoute(
                name: "AllUsersPath",
                url: "users",
                defaults: new { controller = "Users", action = "Index" }
            );

            routes.MapRoute(
                name: "UserUpdate",
                url: "users/{id}",
                defaults: new { controller = "Users", action = "Update" },
                constraints: new { httpmethod = new HttpMethodConstraint("POST") }
            );

            routes.MapRoute(
                name: "User",
                url: "users/{id}/{action}",
                defaults: new { controller = "Users", action = "Index" }
            );

            routes.MapRoute(
                name: "LoginPathGet",
                url: "login",
                defaults: new { controller = "Sessions", action = "New" },
                constraints: new { httpmethod = new HttpMethodConstraint("GET") }
            );

            routes.MapRoute(
                name: "LoginPathPost",
                url: "login",
                defaults: new { controller = "Sessions", action = "Create" },
                constraints: new { httpmethod = new HttpMethodConstraint("POST") }
            );

            routes.MapRoute(
                name: "LogoutPath",
                url: "logout",
                defaults: new { controller = "Sessions", action = "Destroy" }
            );

            routes.MapRoute(
                name: "AccountActivationRoute",
                url: "account_activations/{token}/edit",
                defaults: new { controller = "AccountActivation", action = "Edit" }
            );
            
            routes.MapRoute(
                name: "PasswordResetRoute",
                url: "password_resets/{action}",
                defaults: new { controller = "PasswordResets", action = "Create"}
            );

            routes.MapRoute(
                name: "PasswordResetEditRoute",
                url: "password_resets/{token}/Edit",
                defaults: new { controller = "PasswordResets", action = "Edit" }
            );
            routes.MapRoute(
                name: "PasswordResetUpdateRoute",
                url: "password_resets/{token}/Update",
                defaults: new { controller = "PasswordResets", action = "Update" }
            );
        }
    }
}
