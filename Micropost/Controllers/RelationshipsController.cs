using Microposts.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microposts.Controllers
{
    public class RelationshipsController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        [Route("relationships", Name = "Follow")]
        public async Task<JsonResult> Create(int id)
        {
            var user = await UserManager.FindByIdAsync(id);
            var currentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            currentUser.Follow(user);
            await HttpContext.GetOwinContext().Get<ApplicationDbContext>().SaveChangesAsync();
            
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { success = true, view = this.RenderRazorViewToString("_Unfollow", user), count = user.Followers.Count }, JsonRequestBehavior.AllowGet);
        }
        

        [HttpDelete]
        [Route("relationships/{id}", Name = "Unfollow")]
        public async Task<JsonResult> Destroy(int id)
        {
            var user = await UserManager.FindByIdAsync(id);
            var currentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            currentUser.Unfollow(user);
            await HttpContext.GetOwinContext().Get<ApplicationDbContext>().SaveChangesAsync();

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { success = true, view = this.RenderRazorViewToString("_Follow", user), count = user.Followers.Count }, JsonRequestBehavior.AllowGet);
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}