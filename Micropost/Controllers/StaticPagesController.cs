using Microposts.DataAccess;
using Microposts.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Microposts.Controllers
{
    public class StaticPagesController : Controller
    {
        [HttpGet]
        [Route("",Name="Default")]
        public async Task<ActionResult> Home(int? page)
        {            
            if (Request.IsAuthenticated)
            {
                
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await userManager.FindByIdAsync(User.Identity.GetUserId<int>());

                var context = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                ViewBag.MicropostCount = user.Microposts.Count();

                int pageSize = 25;
                ViewBag.FeedItems = user.Feed().ToPagedList(page ?? 1, pageSize);

                return View();
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        [Route("help",Name = "Help")]
        public ActionResult Help()
        {            
            return View();
        }

        [HttpGet]
        [Route("about",Name = "About")]
        public ActionResult About()
        {            
            return View();
        }

        [HttpGet]
        [Route("contact", Name = "contact")]
        public ActionResult Contact()
        {                    
            return View();
        }
    }
}