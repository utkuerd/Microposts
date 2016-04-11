using System.Web.Mvc;

namespace Micropost.Controllers
{
    public class StaticPagesController : Controller
    {
        [HttpGet]
        [Route("",Name="Default")]
        public ActionResult Home()
        {            
            return View();
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