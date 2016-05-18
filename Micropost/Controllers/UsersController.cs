using PagedList;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace Microposts.Controllers
{
    [Authorize]
    public class UsersController : Controller
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

        [HttpGet]
        [Route("users", Name="AllUsersPath")]
        public ActionResult Index(int? page)
        {
            int pageSize = 25;

            var users = UserManager.Users.OrderBy(user => user.Id);
            return View(users.ToPagedList(page ?? 1, pageSize));
        }

        [HttpGet]
        [Route("users/{id}", Name = "User")]
        public async Task<ActionResult> Show(int id, int? page)
        {
            int pageSize = 25;

            var user = await UserManager.FindByIdAsync(id);
            var microposts = user.Microposts;
            ViewBag.Microposts = microposts.ToPagedList(page ?? 1, pageSize);            
            return View(user);
        }
        
        [HttpDelete]
        [ValidateAntiForgeryToken]
        [Route("users/{id}/delete", Name = "UserDelete")]
        public async Task<ActionResult> Destroy(int id)
        {
            var user = await UserManager.FindByIdAsync(id);
            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["success"] = "User Deleted";
            }
            else
            {
                TempData["error"] = "Unable to Delete User";
            }
            return RedirectToAction("Index", RouteData.Values["page"]);            
        }
    }
}