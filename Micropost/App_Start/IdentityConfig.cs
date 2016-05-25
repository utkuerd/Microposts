using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microposts.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microposts.Models;

namespace Microposts
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        public Task SendRegistrationMail(ApplicationUser user, string registrationToken)
        {
            return new UserMailerController().AccountActivation(user,registrationToken).DeliverAsync();            
        }

        public Task SendForgetPasswordMail(ApplicationUser user, string forgetPasswordCode)
        {
            return new UserMailerController().PasswordReset(user, forgetPasswordCode).DeliverAsync();
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser, int>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, int> store)
            : base(store)
        {

        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new CustomUserStore(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser,int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true,
                
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6
                //RequireNonLetterOrDigit = true,
                //RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = false;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            
            manager.EmailService = new EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser,int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public Task SendRegistrationEmailAsync(ApplicationUser user, string registrationCode)
        {
            return ((EmailService)EmailService).SendRegistrationMail(user, registrationCode);
        }

        public Task SendForgetPasswordEmailAsync(ApplicationUser user, string forgetPasswordCode)
        {
            return ((EmailService) EmailService).SendForgetPasswordMail(user, forgetPasswordCode);
        }

        public async Task<bool> Following(int usr1, int usr2)
        {
            var user1 = await FindByIdAsync(usr1);
            var user2 = await FindByIdAsync(usr2);
            return user1.Following.Contains(user2);
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
