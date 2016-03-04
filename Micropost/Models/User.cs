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
        public string Email {
            get
            {
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
                return BCrypt.Net.BCrypt.HashPassword(Password, BCrypt.Net.BCrypt.GenerateSalt(12));
            }
            set { _PasswordDigest = value; }  
        }

        public bool Authenticate(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, _PasswordDigest);
        }
    }

    public class UserEmailUniqueValidator : AbstractValidator<User>
    {
        public UserEmailUniqueValidator()
        {
            RuleFor(x => x.Email).Must(BeUniqueEmail);
        }

        private bool BeUniqueEmail(string email)
        {
            var _db = new ApplicationDbContext();
            if (_db.CustomUsers.SingleOrDefault(x => String.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase)) == null)
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