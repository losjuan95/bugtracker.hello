using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using bugtracker.Helpers;
using bugtracker.Models;
using Microsoft.AspNet.Identity;

namespace bugtracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelpers projecthelper = new ProjectHelpers();
        private UserRolesHelpers roleHelpers = new UserRolesHelpers();
        
        // GET: Projects
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();
            var userprojects = projecthelper.ListUserProjects(userid).ToList();
            return View(userprojects);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles ="Admin, PM")]
        public ActionResult Create()
        {
            var devs = roleHelpers.UsersInRole("Devs");
            var subs = roleHelpers.UsersInRole("Sub");
            var pms = roleHelpers.UsersInRole("PM");

            ViewBag.ProjManager = new SelectList(pms, "Id", "Email");
            ViewBag.Developers = new MultiSelectList(devs, "Id", "Email");
            ViewBag.Submitters = new MultiSelectList(subs, "Id", "Email");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Project project, List<string> Developers, List<string> Submitters, string ProjManager)
        {
            if (ModelState.IsValid)
            {

               
                db.Projects.Add(project);
                db.SaveChanges();
                if (User.IsInRole("PM"))
                {
                    projecthelper.AddUserToProject(User.Identity.GetUserId(), project.Id);
                }
              
                else
                {
                    if (string.IsNullOrEmpty(ProjManager))
                    {
                        projecthelper.AddUserToProject(ProjManager, project.Id);

                    }
                }
             
                if(Developers != null)
                {
                    foreach (var dev in Developers)
                    {
                        projecthelper.AddUserToProject(dev, project.Id);

                    }
                }
                if(Submitters != null)
                {
                    foreach (var sub in Submitters)
                    {
                        projecthelper.AddUserToProject(sub, project.Id);
                    }

                }
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles ="Admin, PM")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            var list = projecthelper.UsersOnProject(project.Id);
            string CurrentPM = "";
            List<String> CurrentSubs = new List<string>();
            List<String> CurrentDevs = new List<string>();
            var PM = roleHelpers.UsersInRole("PM");
            foreach (var user in list )
            {
                if (roleHelpers.IsUserInRole(user.Id, "PM"))
                { CurrentPM = user.Id; }
            }
            ViewBag.ProjectManager = new SelectList(PM, "Id", "Email", CurrentPM);

            var Dv = roleHelpers.UsersInRole("Devs");
            foreach (var user in list)
            {
                if (roleHelpers.IsUserInRole(user.Id, "Devs"))
                {
                    CurrentDevs.Add(user.Id);
                }
            }

            ViewBag.Developers = new MultiSelectList(Dv, "Id", "Email", CurrentDevs);

            var Subs = roleHelpers.UsersInRole("Sub");
            foreach (var user in list)
            {
                if (roleHelpers.IsUserInRole(user.Id, "Sub"))
                {
                    CurrentSubs.Add(user.Id);
                }
            }
            ViewBag.Submitters = new MultiSelectList(Subs, "Id", "Email", CurrentSubs);
            return View(project);

        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Project project, string ProjectManager, List<String> Submitters, List<String> Developers)
        {
            var projectUsers = projecthelper.UsersOnProject(project.Id);
            if (ModelState.IsValid) 
            {
                if (ProjectManager != null)
                {
                    foreach(var user in projectUsers.ToList())
                    {
                        projecthelper.RemoveUserFromProject(user.Id, project.Id);
                    }
                    projecthelper.AddUserToProject(ProjectManager, project.Id);

                }
                if (Developers != null)
                {
                    foreach (var user in Developers)
                    {
                        projecthelper.AddUserToProject(user, project.Id);
                    }
                  
                }
                if (Submitters != null)
                {
                    foreach (var user in Submitters)
                    {
                        projecthelper.AddUserToProject(user, project.Id);
                    }

                }
                
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
