using bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bugtracker.Helpers;


namespace bugtracker.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public UserRolesHelpers roleHelper = new UserRolesHelpers();
        public ProjectHelpers projectHelper = new ProjectHelpers();


        // GET: Admin
        [Authorize(Roles = "Admin, PM")]
        public ActionResult AssignRole()
        {
            //I want to load up a viewbag property that holds each of the users in my system 

            ViewBag.Users = new SelectList(db.Users, "Id", "Email");
            //ViewBag.Users2 = new SelectList(db.Users, "Id", "FirstName");
            //ViewBag.Users3 = new SelectList(db.Users, "Id", "LastName");



            //I want to load up a viewbag property that holds each of the Roles in my system 
            //ViewBag.Roles1 = new SelectList(db.Roles, "Id", "Name");
            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name");
           


            return View(db.Users.ToList());
        }
        //POST: 
        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignRole(string users, string roles)
        {
            var currentRole = roleHelper.ListUserRoles(users).FirstOrDefault();
            if (!string.IsNullOrEmpty(currentRole))
            {
                roleHelper.RemoveUserFromRole(users, currentRole);
            }
            
            roleHelper.AddUserToRole(users, roles);


            return RedirectToAction("Index", "Projects");
        }

        [HttpGet]
        public ActionResult AssignProjectUsers(int id)
        {


            var currentPm = projectHelper.GetProjectUserRoles("PM", id);
            var pms = roleHelper.UsersInRole("PM");

            ViewBag.ProjectManager = new SelectList(pms, "Id", "Email", currentPm);

            var currentSubs = projectHelper.GetProjectUserRoles("Sub", id);
            var subs = roleHelper.UsersInRole("Sub");

            ViewBag.Submitters = new MultiSelectList(subs, "Id", "Email", currentSubs);

            var currentDevs = projectHelper.GetProjectUserRoles("Devs", id);
            var devs = roleHelper.UsersInRole("Devs");

            ViewBag.Developers = new MultiSelectList(devs, "Id", "Email", currentDevs);




            return View(db.Projects.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult AssignProjectUsers(int id, string ProjectManager, List<string> Submitters, List<string> Developers)
        {
            //Before we assign anybody to the project we first need to unassign everybody from the project

           foreach (var user in projectHelper.UsersOnProject(id).ToList())
            {
                projectHelper.RemoveUserFromProject(user.Id, id);
            }

            projectHelper.AddUserToProject(ProjectManager, id);
            
            if(Submitters != null)
            {
                foreach (var user in Submitters)
                {
                    projectHelper.AddUserToProject(user, id);
                }
            }

            if (Developers != null)
            {
                foreach (var user in Developers)
                {
                    projectHelper.AddUserToProject(user, id);
                }
            }
            
            return View();
        }

    }
}