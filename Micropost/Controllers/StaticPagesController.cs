using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Micropost.Controllers
{
    public class StaticPagesController : Controller
    {
        public ActionResult Home()
        {            
            return View();
        }

        public ActionResult Help()
        {            
            return View();
        }

        public ActionResult About()
        {            
            return View();
        }

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}