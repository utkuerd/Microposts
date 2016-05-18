using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microposts.Models;
using Microsoft.AspNet.Identity;

namespace Microposts.Helper
{
    public class SelfAuthorizeAttribute: AuthorizeAttribute
    {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                var authorized = base.AuthorizeCore(httpContext);
                if (!authorized)
                {
                    return false;
                }

                var rd = httpContext.Request.RequestContext.RouteData;

                var id = Convert.ToInt32(rd.Values["id"]);
                var loggedInUserId = httpContext.User.Identity.GetUserId<int>();

                return id == loggedInUserId;
            }
        
    }
}