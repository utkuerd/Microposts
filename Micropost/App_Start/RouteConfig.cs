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
                defaults: new { controller = "User", action = "New" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "StaticPages", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}
