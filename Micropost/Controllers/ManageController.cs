using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CsQuery.ExtensionMethods.Internal;
using Microposts.Helper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microposts.Models;

namespace Microposts.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
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

        [HttpGet]
        [Route("users/{id}/edit", Name = "UserEditForm")]
        public async Task<ViewResult> UpdateProfile(int id)
        {
            var user = await UserManager.FindByIdAsync(id);
            
            var configuration = new MapperConfiguration(config => { });
            var profileExpression = configuration as IProfileExpression;

            profileExpression.CreateMap<ApplicationUser, UpdateProfileViewModel>();

            var autoMapper = configuration.CreateMapper();

            var viewModel = autoMapper.Map<UpdateProfileViewModel>(user);

            return View(viewModel);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelfAuthorize]
        [Route("users/{id}/edit", Name = "UserEdit")]
        public async Task<ActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());

            if (!model.FullName.IsNullOrEmpty())
            {
                user.FullName = model.FullName;
            }

            if (!model.Email.IsNullOrEmpty())
            {
                user.Email = model.Email;
            }

            await UserManager.UpdateAsync(user);

            if (model.NewPassword != null && model.NewPassword.Equals(model.ConfirmPassword))
            {
                var token = await UserManager.GeneratePasswordResetTokenAsync(User.Identity.GetUserId<int>());
                var result =
                    await UserManager.ResetPasswordAsync(User.Identity.GetUserId<int>(), token, model.NewPassword);

                if (!result.Succeeded)
                {
                    AddErrors(result);
                    return View(model);
                }
            }

            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            TempData["success"] = "Profile updated.";
            return RedirectToRoute("User", new { id = user.Id, action = "Show" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
#endregion
    }
}