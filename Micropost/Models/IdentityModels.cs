using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace Microposts.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        public virtual ICollection<Micropost> Microposts { get; set; } = new List<Micropost>();
        public virtual ICollection<ApplicationUser> Following { get; set; } = new List<ApplicationUser>();
        public virtual ICollection<ApplicationUser> Followers { get; set; } = new List<ApplicationUser>();

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("FullName", this.FullName));
            userIdentity.AddClaim(new Claim("FollowingCount", this.Following.Count().ToString()));
            userIdentity.AddClaim(new Claim("FollowerCount", this.Followers.Count().ToString()));    
           
            return userIdentity;
        }

        internal void Unfollow(ApplicationUser user)
        {
            Following.Remove(user);
            user.Followers.Remove(this);
        }

        public IEnumerable<Micropost> Feed()
        {
            var following = Following.Select(u => u.Id).ToList();
            var query = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>().Microposts.Where(mp => mp.User.Id == this.Id || following.Contains(mp.User.Id) )
                                .OrderByDescending(mp => mp.CreatedAt);
            return query;
        }

        internal void Follow(ApplicationUser user)
        {
            Following.Add(user);
            user.Followers.Add(this);            
        }
    }

    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}