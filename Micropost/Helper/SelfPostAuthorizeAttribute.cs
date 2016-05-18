using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microposts.Models;
using Microsoft.AspNet.Identity;
using Microposts.DataAccess;
using Microsoft.AspNet.Identity.Owin;

namespace Microposts.Helper
{
    public class SelfPostAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            if (!authorized)
            {
                return false;
            }

            var micropostId = Convert.ToInt32(httpContext.Request.Params.Get("micropostId"));
            var MicropostRepository = new MicropostRepository(httpContext.GetOwinContext().Get<ApplicationDbContext>());
            var postUserId = MicropostRepository.FindById(micropostId).User.Id;
            
            var userId = httpContext.User.Identity.GetUserId<int>();


            return postUserId == userId;
        }
    }
}