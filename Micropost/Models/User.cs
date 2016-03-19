using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micropost.Models
{
    using BCrypt.Net;
    using Controllers;
    using System.Diagnostics;
    [Validator(typeof(UserEmailUniqueValidator))]
    public class User
    {
        private const string EmailRegexp = @"\A[\w+\-.]+@[A-Za-z\d\-.]+\.[A-Za-z]+\z";

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        private string _EMail;
        [Required]
        [StringLength(254)]
        [RegularExpression(EmailRegexp)] 
        [Index(IsUnique =true)]                               
        public string Email {
            get
            {
                if (_EMail == null) return null;
                return _EMail.ToLower();
            }
            set { _EMail = value; }
        }        

        [MinLength(6)]
        [NotMapped]
        public string Password { get; set; }

        [MinLength(6)]
        [Compare("Password")]        
        [NotMapped]
        public string PasswordConfirmation { get; set; }

        private string _PasswordDigest;
        public string PasswordDigest {
            get
            {
                if (_PasswordDigest != null)
                {
                    return _PasswordDigest;
                }   
                else if (Password != null)
                {
                    return BCrypt.HashPassword(Password, BCrypt.GenerateSalt(12));
                }             
                else
                {
                    return null;
                }
            }
            set { _PasswordDigest = value; }  
        }

        [NotMapped]
        public string RememberToken { get; set; }
        public string RememberDigest { get; set; }
        
        public bool Admin { get; set; }

        [NotMapped]
        public string ActivationToken { get; set; }
        public string ActivationDigest { get; set; }
        public bool Activated { get; set; }        
        public DateTime? ActivatedAt { get; set; }

        [NotMapped]
        public string ResetToken { get; set; }
        public string ResetDigest { get; set; }        
        public DateTime? ResetSentAt { get; set; }

        public User()
        {
            CreateActivationDigest();
        }


        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            User p = obj as User;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return Id == p.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool Authenticate(string password)
        {
            return BCrypt.Verify(password, _PasswordDigest);
        }

        public bool TokenAuthenticated(string attribute,string token)
        {
            Type t = typeof(User);

            var property = t.GetProperty(attribute + "Digest");

            if (property == null) return false;

            string digest = property.GetValue(this) as string;

            if (digest == null) return false;

            return BCrypt.Verify(token, digest);
        }

        public void Remember()
        {
            RememberToken = User.NewToken();
            RememberDigest = User.Digest(RememberToken);
        }

        private void CreateActivationDigest()
        {
            ActivationToken = User.NewToken();
            ActivationDigest = User.Digest(ActivationToken);
        }

        public void Forget()
        {
            RememberDigest = null;
        }

        public static string Digest(string clearText)
        {        
            return BCrypt.HashPassword(clearText);
        }

        public static string NewToken()
        {
            byte[] linkBytes = new byte[12];
            var rngCrypto = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rngCrypto.GetBytes(linkBytes);
            var text = Convert.ToBase64String(linkBytes);
            var textEnc = text.TrimEnd(new char[] { '=' }).Replace('+','-').Replace('/','_');

            return textEnc;
        }      
        
        public void Activate()
        {
            Activated = true;
            ActivatedAt = DateTime.Now;
        }  

        public void SendActivationLink()
        {
            var email = new UserMailerController().AccountActivation(this);            

            //email.Deliver();
        }

        internal void SendPasswordResetEmail()
        {
            var email = new UserMailerController().PasswordReset(this);

            email.Deliver();
        }

        internal void CreateResetDigest()
        {
            ResetToken = User.NewToken();
            ResetDigest = User.Digest(ResetToken);
            ResetSentAt = DateTime.Now;
        }

        internal bool PasswordResetExpired()
        {
            return ResetSentAt < DateTime.Now - new TimeSpan(2, 0, 0);
        }
    }

    public class UserEmailUniqueValidator : AbstractValidator<User>
    {
        public UserEmailUniqueValidator()
        {
            RuleFor(x => x.Email).Must(BeUniqueEmail).WithMessage("Email address should be unique.");
        }

        private bool BeUniqueEmail(User u, string email)
        {
            var _db = new ApplicationDbContext();
            if (_db.CustomUsers.FirstOrDefault(x => x.Email.Equals(email,StringComparison.OrdinalIgnoreCase) && x.Id != u.Id ) == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}