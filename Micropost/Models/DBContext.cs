using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Microposts.Models
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        } 
        
        public DbSet<Micropost> Microposts { get; set; }       

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {   
            base.OnModelCreating(modelBuilder);     
            modelBuilder.Entity<CustomRole>().Property(r => r.Id).
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

                modelBuilder.Entity<ApplicationUser>()
                    .HasMany(user => user.Following)
                    .WithMany(user => user.Followers)
                    .Map(m =>
                        {
                            m.ToTable("Relationships");
                            m.MapLeftKey("FollowerId");
                            m.MapRightKey("FollowedId");                        
                        }
                    );
        }
    }
}