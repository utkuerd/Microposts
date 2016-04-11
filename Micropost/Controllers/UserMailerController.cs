using ActionMailerNext.Mvc5_2;
using Micropost.Models;
using System.Net.Mail;
using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Micropost.Controllers
{
    public class UserMailerController : MailerBase
    {
        public UserMailerController()
        {
            MailAttributes.From = new MailAddress("noreply@example.com");
        }
        
        public EmailResult AccountActivation(ApplicationUser user, string code)
        {
            MailAttributes.To.Add(new MailAddress(user.Email));
            MailAttributes.Subject = "Account activation";

            ViewBag.Code = code;
            return Email("AccountActivation",user);
        }

        public EmailResult PasswordReset(ApplicationUser user, string code)
        {
            MailAttributes.To.Add(new MailAddress(user.Email));
            MailAttributes.Subject = "Password reset";

            ViewBag.Code = code;
            return Email("PasswordReset", user);
        }

    }
}