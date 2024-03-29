﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using bugtracker.Models;
using Microsoft.AspNet.Identity;
using bugtracker.Helpers;
using PagedList;
using PagedList.Mvc;


namespace bugtracker.Controllers
{
    [Authorize(Roles="Admin, Sub, PM, Devs")]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelpers Projecthelper = new ProjectHelpers();
        private UserRolesHelpers Userrolehelper = new UserRolesHelpers();
        // GET: Tickets

        public ActionResult Index()
        {
            
            var VM = new TicketsViewModel();
            var currentUserId = User.Identity.GetUserId();
            VM.AllTickets.AddRange(db.Tickets.ToList());
            if (User.IsInRole("Sub"))
            {
                VM.SubTickets = VM.AllTickets.Where(t => t.OwnerUserId == currentUserId).ToList();
            }
            if (User.IsInRole("Devs"))
            {
                VM.DevTickets = VM.AllTickets.Where(t => t.AssignedToUserId == currentUserId).ToList();
            }
            if (User.IsInRole("Devs")|| User.IsInRole("PM"))
            {
                var MyProjects = Projecthelper.ListUserProjects(currentUserId);
                foreach(var project in MyProjects)
                {
                    VM.ProjTickets.AddRange(project.Tickets);
                }

            }

            return View(VM);
        }

        // GET: Tickets/Details/5
        [Authorize(Roles = "Admin, Sub, PM, Devs")]

        public ActionResult Details(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();

            }
            if (User.IsInRole("Devs") && User.Identity.GetUserId() != ticket.AssignedToUserId)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (User.IsInRole("Sub")&& User.Identity.GetUserId()!= ticket.OwnerUserId)
            {
            return RedirectToAction("Index", "Manage");
            }
            return View(ticket);
            
        }

        // GET: Tickets/Create
        [Authorize(Roles ="Admin, Sub")]
        public ActionResult Create(int? Id)
        {
            var userprojects = Projecthelper.ListUserProjects(User.Identity.GetUserId());
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            if(Id == null)
            {
                ViewBag.ProjectId = new SelectList(userprojects, "Id", "Name");

            }
            else
            {
                ViewBag.ProjectId = new SelectList(userprojects, "Id", "Name", Id);

            }
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId= new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");

            var ticket = new Ticket
            {
                TicketTypeId = db.TicketTypes.FirstOrDefault(t => t.Name == "Bug").Id,
                TicketPriorityId = db.TicketPriorities.FirstOrDefault(p => p.Name == "Low").Id,
                TicketStatusId = db.TicketStatuses.FirstOrDefault(s => s.Name == "Un-Assigned").Id
            };

            return View(ticket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.Created = DateTime.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin, Sub, PM, Devs")]
         public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Devs") && User.Identity.GetUserId() != ticket.AssignedToUserId)
            {
                return RedirectToAction("Index", "Manage");
            }
            var devhelper = Userrolehelper.UsersInRole("Devs");
            ViewBag.AssignedToUserId = new SelectList(devhelper, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,Title,Description,Created,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {

                //getting a reference to the old ticket somehow
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                
                ticket.Updated = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                var notificationHelper = new NotificationHelper();
                notificationHelper.Notify(oldTicket, ticket);
                var historyHelper = new HistoryHelper();
                historyHelper.AddHistory(oldTicket, ticket);

                
                return RedirectToAction("Index", "Tickets");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
