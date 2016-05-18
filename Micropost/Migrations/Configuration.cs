using Microsoft.AspNet.Identity;

namespace Microposts.Migrations
{
    using Faker;
    using Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;            
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var userStore = new CustomUserStore(context); 
            var userManager = new ApplicationUserManager(userStore); 
            userManager.UserValidator = new UserValidator<ApplicationUser,int>(userManager) {AllowOnlyAlphanumericUserNames = false};

            if (!context.Roles.Any(role => role.Name.Equals("Admin")))
            {
                var roleStore = new CustomRoleStore(context);
                var roleManager = new  RoleManager<CustomRole,int>(roleStore);    
                var adminRole = new CustomRole("Admin");

                roleManager.Create(adminRole);
            }

            if (!context.Users.Any(user => user.Email.Equals("example@railstutorial.org")))
            {                
                var first = new ApplicationUser()
                {
                    FullName = "Example User",
                    Email = "example@railstutorial.org",                    
                    UserName = "example@railstutorial.org",
                    EmailConfirmed = true
                };

                userManager.Create(first, "foobar");
                userManager.AddToRole(first.Id, "Admin");
            }            

            for (int i = 0; i < 99; i ++)
            {
                var name = Name.FullName();
                var email = $"example-{i + 1}@railstutorial.org";
                const string password = "password";

                var newUser = new ApplicationUser()
                {
                    FullName = name,
                    Email = email,
                    UserName =  email,
                    EmailConfirmed = true
                };

                if (!context.Users.Any(user => user.Email.Equals(email)))
                {
                    var result = userManager.Create(newUser, password);
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n",result.Errors));
                    }
                }
            }

            var users = context.Users.OrderBy(user => user.Id).Take(6).ToList();
            for (int i = 0; i < 50; i++)
            {
                var content = Lorem.Sentence(5);
                foreach(var user in users)
                {
                    Micropost m = new Micropost { Content = content, User = user, CreatedAt = DateTime.Now };
                    user.Microposts.Add(m);
                }
            }          
        }       
    }
}
