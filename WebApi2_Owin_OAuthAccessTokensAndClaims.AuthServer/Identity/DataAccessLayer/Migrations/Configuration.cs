namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.DataAccessLayer.Migrations
{
    using Contexts;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.Identity.Entities;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Identity\DataAccessLayer\Migrations";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            /*----------------------------------------------------------------------------------------------------------
                Check that we do not already have data persisted in the database
            ----------------------------------------------------------------------------------------------------------*/
            var count = (from peeps in context.People select (peeps)).Count();
            if (count > 0) { return; }


            /*----------------------------------------------------------------------------------------------------------
                Create some Person objects
            ----------------------------------------------------------------------------------------------------------*/
            Person[] people =
            {
                new Person { FirstName = "Paul", LastName = "Millar", Level = 1, JoinDate = DateTime.Now.AddYears(-3)},
                new Person { FirstName = "Mary", LastName = "Mackin", Level = 1, JoinDate = DateTime.Now.AddYears(-1)},
                new Person { FirstName = "Jane", LastName = "Doe", Level = 3, JoinDate = DateTime.Now}
            };


            /*----------------------------------------------------------------------------------------------------------
                Create some 'ApplicationUser' objects
            ----------------------------------------------------------------------------------------------------------*/
            ApplicationUser[] appUsers =
            {
                new ApplicationUser { UserName = "millime", Email = "paulj@whateverco.ie", Person = people[0] },
                new ApplicationUser { UserName = "macka", Email = "macka@whateverco.ie", Person = people[1] },
                new ApplicationUser { UserName = "doedoe", Email = "jdoe@whateverco.ie", Person = people[2] }
            };


            /*----------------------------------------------------------------------------------------------------------
                Create some 'Manager' objects
            ---------------------------------------------------------------------------------------------------------- */
            //var dbContext = new ApplicationDbContext();
            var appManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));


            /*----------------------------------------------------------------------------------------------------------
                Persist users to db
            ----------------------------------------------------------------------------------------------------------*/
            appManager.Create(appUsers[0], "P@ssword!");
            appManager.Create(appUsers[1], "Pazzword!");
            appManager.Create(appUsers[2], "P@ssw0rd!");


            /*----------------------------------------------------------------------------------------------------------
                Create some roles
            ----------------------------------------------------------------------------------------------------------*/
            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }


            /*----------------------------------------------------------------------------------------------------------
                Assign roles
            ----------------------------------------------------------------------------------------------------------*/
            var user1 = appManager.FindByName("millime");
            var user2 = appManager.FindByName("macka");
            var user3 = appManager.FindByName("doedoe");

            appManager.AddToRoles(user1.Id, new string[] { "SuperAdmin", "Admin", "User" });
            appManager.AddToRoles(user2.Id, new string[] { "Admin", "User" });
            appManager.AddToRoles(user3.Id, new string[] { "User" });
        }
    }
}
