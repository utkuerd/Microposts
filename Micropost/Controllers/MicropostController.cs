using Microposts.ViewModels;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microposts.DataAccess;
using Microposts.Models;
using System.Linq;
using PagedList;
using Backload.Contracts.FileHandler;
using System;
using System.IO;
using Flurl;
using Humanizer;

namespace Microposts.Controllers
{
    [Authorize]
    public class MicropostController : Controller
    {
        private ApplicationUserManager _userManager;
        private MicropostRepository _micropostRepository;
        private int _newMPId;

        public MicropostRepository MicropostRepository
        {
            get
            {
                return _micropostRepository ?? new MicropostRepository(HttpContext.GetOwinContext().Get<ApplicationDbContext>());
            }
            private set
            {
                _micropostRepository = value;
            }
        }

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
        [Route("microposts",Name="MicropostCreate")]     
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewMicropost newMP)
        {
            var currentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (ModelState.IsValid)
            {
                _newMPId = MicropostRepository.Create(newMP.TweetContent, currentUser);

                IFileHandler handler = Backload.FileHandler.Create();
                handler.Events.StoreFileRequestStarted += Events_StoreFileRequestStarted;
                handler.Init(HttpContext.Request);
                var result = await handler.Execute();

                if (handler.FileStatus.Files[0].Success)
                {
                    var request = HttpContext.Request;
                    var absoluteRoot = request.Url.AbsoluteUri.Replace(request.Url.AbsolutePath, String.Empty);
                    var relativeURL = handler.FileStatus.Files[0].FileUrl.Replace(absoluteRoot, "");

                    MicropostRepository.AttachPicture(_newMPId, relativeURL);
                }
                else
                {
                    TempData["error"] = handler.FileStatus.Files[0].ErrorMessage;
                    ViewBag.FeedItems = Enumerable.Empty<Micropost>().ToPagedList(1, 25); ;
                    ViewBag.MicropostCount = currentUser.Microposts.Count;
                    return View("../StaticPages/Home", newMP);
                }

                MicropostRepository.Save();
                TempData.Add("success", "Micropost created.");
                return RedirectToRoute("Default");     
            }
            else
            {                
                ViewBag.FeedItems = Enumerable.Empty<Micropost>().ToPagedList(1, 25); ;
                ViewBag.MicropostCount = currentUser.Microposts.Count;
                return View("../StaticPages/Home", newMP);
            }            
        }

        private void Events_StoreFileRequestStarted(IFileHandler sender, Backload.Contracts.Eventing.IStoreFileRequestEventArgs e)
        {
            var file = e.Param.FileStatusItem;
            var si = file.StorageInfo;
            var sizeLimit = (5).Megabytes();

            if (! new string[] { ".png", ".gif", ".jpg", ".jpeg" }.Contains(Path.GetExtension(file.FileName)) )
            { 
                file.FileData = null;                       // Setting FileData to null is a shortcut to force Backload not to store the file.
                file.ErrorMessage = "Invalid file type"; // This message will be displayed in the client side plugin
                file.Success = false;
            }
            else if (file.FileSize > sizeLimit.Bytes)
            {
                file.FileData = null;                   
                file.ErrorMessage = file.FileName+ " should be less then "+ sizeLimit.Humanize();
                file.Success = false;
            }
            else
            {

                var uploadDir = Server.MapPath("~/Files");
                var fullPath = Path.Combine(uploadDir, Convert.ToString(_newMPId), file.FileName);
                Directory.CreateDirectory(Path.Combine(fullPath, ".."));

                var request = HttpContext.Request;            
                e.Param.FileStatusItem.StorageInfo.FilePath = fullPath;
                e.Param.FileStatusItem.FileUrl = Flurl.Url.Combine(request.Url.AbsoluteUri.Replace(request.Url.AbsolutePath, "/Files"),
                                                                    Convert.ToString(_newMPId),
                                                                    file.FileName);
            }
        }

        [HttpDelete]
        [Route("microposts/{micropostId}", Name = "MicropostDelete")]
        public ActionResult Destroy(int micropostId)
        {
            MicropostRepository.Delete(micropostId);
            MicropostRepository.Save();

            TempData["success"] = "Micropost deleted";           
            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                return RedirectToRoute("Default");
            }
        }
    }
}