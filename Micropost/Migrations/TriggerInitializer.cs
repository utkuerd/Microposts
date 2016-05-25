using Microposts.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Microposts.Migrations
{
    public class TriggerInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            context.Database.ExecuteSqlCommand(@"CREATE TRIGGER [Delete_User_Trigger] 
                                                ON dbo.AspNetUsers 
                                                INSTEAD OF DELETE 
                                                AS
                                                BEGIN
                                                    SET NOCOUNT ON;
                                                    DELETE FROM dbo.Relationships 
                                                    WHERE FollowerId IN (SELECT Id FROM DELETED) OR
                                                            FollowedId IN (SELECT Id FROM DELETED);

                                                    DELETE FROM dbo.AspNetUsers
                                                    WHERE Id IN (SELECT Id FROM DELETED);

                                                END");
        }
    }
}