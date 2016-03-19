using ActionMailerNext.Mvc5_2;
using Micropost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Micropost.Controllers
{
    public class UserMailerController : MailerBase
    {
        public UserMailerController()
        {
            MailAttributes.From = new MailAddress("noreply@example.com");
        }

        // GET: UserMailer
        public EmailResult AccountActivation(User user)
        {
            MailAttributes.To.Add(new MailAddress(user.Email));
            MailAttributes.Subject = "Account activation";

            return Email("AccountActivation",user);
        }

        public EmailResult PasswordReset(User user)
        {
            MailAttributes.To.Add(new MailAddress(user.Email));
            MailAttributes.Subject = "Password reset";

            return Email("PasswordReset", user);
        }
    }
}