using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microposts.Models;

namespace Microposts.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
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
                
        [Route("login", Name = "LoginForm")]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
                
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("login")]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Require the user to have a confirmed email before they can log on.
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    TempData["warning"] = "Account not activated. Check your email for activation link. ";
                    return RedirectToRoute("Default");
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    TempData["danger"] = "Invalid email/password combination";
                    //ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("signup", Name="SignUpForm")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("signup", Name = "SignUp")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //  await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    await UserManager.SendRegistrationEmailAsync(user, code);

                    TempData["info"] = "Please check your email to activate your account.";
                    return RedirectToRoute("Default");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        [HttpGet]
        [AllowAnonymous]
        [Route("user/{userId}/activate",Name="AccountActivationRoute")]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId == default(int) || code == null)
            {
                TempData["danger"] = "Invalid activation link";
                return RedirectToRoute("Default");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);

            if (!result.Succeeded)
            {
                TempData["danger"] = "Invalid activation link";
                return RedirectToRoute("Default");
            }

            var user = await UserManager.FindByIdAsync(userId);

            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            TempData["success"] = "Account activated!";
            return RedirectToRoute("User", new { id = userId, action= "Show"});
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("password_resets/create",Name="PasswordResetForm")]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("password_resets", Name="PasswordResetEmailSend")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {            
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    TempData["danger"] = "Email address not found or not confirmed";
                    return View();
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                await UserManager.SendForgetPasswordEmailAsync(user, code);
                TempData["info"] = "Email sent with password reset instructions";
                return RedirectToRoute("Default");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        [HttpGet]
        [AllowAnonymous]
        [Route("users/{id}/reset_password", Name = "PasswordResetsReturnForm")]
        public ActionResult ResetPassword(int id, string code)
        {
            var viewModel = new ResetPasswordViewModel();            
            viewModel.Code = code;

            return  View(viewModel);
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("users/{id}/reset_password", Name="PasswordReset")]
        public async Task<ActionResult> ResetPassword(int id, ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToRoute("Default");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                TempData["success"] = "Password has been reset";
                return RedirectToRoute("User", new {id = user.Id, action="Show"});
            }
            AddErrors(result);
            return View();
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        [Route("logout",Name="Logout")]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToRoute("Default");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToRoute("Default");
        }        
        #endregion

    }
}