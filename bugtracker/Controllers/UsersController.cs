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

namespace bugtracker.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelpers roleHelper = new UserRolesHelpers();

        // GET: Users
        public ActionResult Index()
        {
            //[Authorize(Roles = "Admin")]
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,AvatarPath,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser, string roles) 
        {
            if (ModelState.IsValid)
            {
                var currentRoles = roleHelper.ListUserRoles(applicationUser.Id);

                foreach (var role in currentRoles)
                {
                    roleHelper.RemoveUserFromRole(applicationUser.Id, role);
                }
                if(!string.IsNullOrEmpty(roles))
                     roleHelper.AddUserToRole(applicationUser.Id, roles);


                db.Users.Add(applicationUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }



            //since i want to manage the users role i need to allow a role to be sellected
            var Currentrole = roleHelper.ListUserRoles(id).FirstOrDefault();
            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name", Currentrole);


            return View(applicationUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,AvatarPath,Email,PhoneNumber")] ApplicationUser applicationUser, string roles)
        {
            if (ModelState.IsValid)
            {

                var currentroles = roleHelper.ListUserRoles(applicationUser.Id);
                foreach (var role in currentroles)
                {
                    roleHelper.RemoveUserFromRole(applicationUser.Id, role);

                }
                if (!string.IsNullOrEmpty(roles))
                {
                    roleHelper.AddUserToRole(applicationUser.Id, roles);
                }


                applicationUser.UserName = applicationUser.Email;

                db.Users.Attach(applicationUser);
                db.Entry(applicationUser).Property(x => x.FirstName).IsModified = true;
                db.Entry(applicationUser).Property(x => x.LastName).IsModified = true;
                db.Entry(applicationUser).Property(x => x.AvatarPath).IsModified = true;
                db.Entry(applicationUser).Property(x => x.Email).IsModified = true;
                db.Entry(applicationUser).Property(x => x.PhoneNumber).IsModified = true;
                db.Entry(applicationUser).Property(x => x.UserName).IsModified = true;



                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
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
