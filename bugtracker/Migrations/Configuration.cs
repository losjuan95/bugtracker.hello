namespace bugtracker.Migrations
{
    using bugtracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<bugtracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(bugtracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.TicketStatuses.AddOrUpdate(m => m.Name,
                new TicketStatus() { Name = "Open"},
                new TicketStatus() { Name = "Closed" },
                new TicketStatus() { Name = "On-Hold" },
                new TicketStatus() { Name = "Un-Assigned"}

                );
            context.TicketPriorities.AddOrUpdate(m => m.Name,
                new TicketPriority() { Name = "High" },
                new TicketPriority() { Name = "Low" },
                new TicketPriority() { Name = "Medium" }
                );
            context.TicketTypes.AddOrUpdate(m => m.Name,
                new TicketType() { Name = "Bug"},
                new TicketType() { Name = "Documentation"},
                new TicketType() { Name = "Feature"}
                );

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Devs"))
            {
                roleManager.Create(new IdentityRole { Name = "Devs" });
            }

            if (!context.Roles.Any(r => r.Name == "Sub"))
            {
                roleManager.Create(new IdentityRole { Name = "Sub" });
            }

            if (!context.Roles.Any(r => r.Name == "PM"))
            {
                roleManager.Create(new IdentityRole { Name = "PM" });
            }


            //create as few users in the project
            var userManager = new UserManager<ApplicationUser>(
               new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "juancarloscorea95@gmail.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "juancarloscorea95@gmail.com",
                    Email = "juancarloscorea95@gmail.com",
                    FirstName = "Juan Carlos",
                    LastName = "Corea"


                }, "Hollirose95");
                var userId = userManager.FindByEmail("juancarloscorea95@gmail.com").Id;
                userManager.AddToRole(userId, "Admin");
            }
                //Setup demo Users here...
            if (!context.Users.Any(u => u.Email == "DemoAdmin@Mailinator.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoAdmin@Mailinator.com",
                    Email = "DemoAdmin@Mailinator.com",
                    FirstName = "Admin",
                    LastName = "Demo"


                }, "Abc&123!");
                var userId = userManager.FindByEmail("DemoAdmin@Mailinator.com").Id;
                userManager.AddToRole(userId, "Admin");


            }

            if (!context.Users.Any(u => u.Email == "DemoSub@Mailinator.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoSub@Mailinator.com",
                    Email = "DemoSub@Mailinator.com",
                    FirstName = "Submitter",
                    LastName = "Demo"


                }, "Abc&123!");
                var userId = userManager.FindByEmail("DemoSub@Mailinator.com").Id;
                userManager.AddToRole(userId, "Sub");

            }

            if (!context.Users.Any(u => u.Email == "DemoPM@Mailinator.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoPM@Mailinator.com",
                    Email = "DemoPM@Mailinator.com",
                    FirstName = "Project Manager",
                    LastName = "Demo"


                }, "Abc&123!");
                var userId = userManager.FindByEmail("DemoPM@Mailinator.com").Id;
                userManager.AddToRole(userId, "PM");


            }

            if (!context.Users.Any(u => u.Email == "DemoDevs@Mailinator.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoDevs@Mailinator.com",
                    Email = "DemoDevs@Mailinator.com",
                    FirstName = "Developer",
                    LastName = "Demo"


                }, "Abc&123!");
                var userId = userManager.FindByEmail("DemoDevs@Mailinator.com").Id;
                userManager.AddToRole(userId, "Devs");


            }
        }     
    }
}

