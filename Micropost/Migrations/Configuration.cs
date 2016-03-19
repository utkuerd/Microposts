namespace Micropost.Migrations
{
    using Faker;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Micropost.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Micropost.Models.ApplicationDbContext context)
        {
            ResetDB(context.Database);


            User first = new User()
            {
                Name = "Example User",
                Email = "example@railstutorial.org",
                Password = "foobar",
                PasswordConfirmation = "foobar",
                Admin = true,
                Activated = true,
                ActivatedAt = DateTime.Now
            };

            context.CustomUsers.AddOrUpdate(first);

            for (int i = 0; i < 99; i ++)
            {
                var name = Name.FullName();
                var email = String.Format("example-{0}@railstutorial.org", i + 1);
                var password = "password";

                User newUser = new User()
                {
                    Name = name,
                    Email = email,
                    Password = password,
                    PasswordConfirmation = password,
                    Activated = true,
                    ActivatedAt = DateTime.Now
                };

                context.CustomUsers.AddOrUpdate(newUser);
            }

            context.SaveChanges();
        }

        private void ResetDB(Database database)
        {
            database.ExecuteSqlCommand("DELETE FROM Users");
            database.ExecuteSqlCommand("DBCC CHECKIDENT('Users', RESEED, 0)");
        }
    }
}
