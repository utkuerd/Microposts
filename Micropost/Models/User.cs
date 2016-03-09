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

        [Required]
        [MinLength(6)]
        [NotMapped]
        public string Password { get; set; }

        [Required]
        [MinLength(6)]
        [Compare("Password")]        
        [NotMapped]
        public string PasswordConfirmation { get; set; }

        private string _PasswordDigest;
        public string PasswordDigest {
            get
            {
                return _PasswordDigest ?? BCrypt.HashPassword(Password, BCrypt.GenerateSalt(12));
            }
            set { _PasswordDigest = value; }  
        }

        public string RememberToken { get; set; }
        public string RememberDigest { get; set; }



        public bool Authenticate(string password)
        {
            return BCrypt.Verify(password, _PasswordDigest);
        }

        public bool TokenAuthenticated(string token)
        {
            if (RememberDigest == null) return false;

            return BCrypt.Verify(token, RememberDigest);
        }

        public void Remember()
        {
            RememberToken = User.NewToken();
            RememberDigest = User.Digest(RememberToken);
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
            var textEnc = Uri.EscapeDataString(text);

            return textEnc;
        }
    }

    public class UserEmailUniqueValidator : AbstractValidator<User>
    {
        public UserEmailUniqueValidator()
        {
            RuleFor(x => x.Email).Must(BeUniqueEmail).WithMessage("Email address should be unique.");
        }

        private bool BeUniqueEmail(string email)
        {
            var _db = new ApplicationDbContext();
            if (_db.CustomUsers.FirstOrDefault(x => x.Email.Equals(email,StringComparison.OrdinalIgnoreCase)) == null)
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