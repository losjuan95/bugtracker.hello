using bugtracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bugtracker.Helpers
{
    public class TicketHelper
    {
        static ApplicationDbContext db = new ApplicationDbContext();
        static UserRolesHelpers roleHelper = new UserRolesHelpers();
        static ProjectHelpers projHelper = new ProjectHelpers();

        public bool IsUserAssignedToTicket(string userId, int ticketId)
        {
            var ticket = db.Tickets.Find(ticketId);
            var ship = ticket.OwnerUserId.Any();
            return (ship);
        }
        
        public static List<ApplicationUser> MembersList(int id)
        {
            return db.Tickets.Find(id).Project.Users.ToList();
        }

        public static int GetTicketCountByStatus(string status)
        {
            var myUserId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(myUserId).FirstOrDefault();
            var ticketCnt = 0;
            switch (myRole)
            {
                case "Admin":
                   ticketCnt = db.Tickets.Where(t => t.TicketStatus.Name == status).Count();
                    break;
                case "PM":
                    var myProjects = projHelper.ListUserProjects(myUserId);
                    foreach (var project in myProjects)
                    {
                       foreach(var ticket in project.Tickets)
                        {
                            if(ticket.TicketStatus.Name == status)
                            {
                                ticketCnt += 1;
                            }
                        }
                    }
                    break;
                case "Sub":
                    ticketCnt = db.Tickets.Where(t => t.OwnerUserId == myUserId && t.TicketStatus.Name == status).Count();
                    break;
                case "Dev":
                    ticketCnt = db.Tickets.Where(t => t.AssignedToUserId == myUserId && t.TicketStatus.Name == status).Count();
                    break;
                default:
                    break;
            }
            return ticketCnt;
           
        }

        public static string GetTicketPlacementByStatus(string status)
        {
            decimal ticketCnt = 0;
            var myUserId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(myUserId).FirstOrDefault();
            switch (myRole)
            {
                case "Admin":
                    ticketCnt = db.Tickets.Count();
                    break;
                case "PM":
                    var myProjects = projHelper.ListUserProjects(myUserId);
                    foreach (var project in myProjects)
                    {
                        foreach (var ticket in project.Tickets)
                        {
                            ticketCnt += 1;
                        }
                    }
                    break;
                case "Sub":
                    ticketCnt = db.Tickets.Where(t => t.OwnerUserId == myUserId).Count();
                    break;
                case "Dev":
                    ticketCnt = db.Tickets.Where(t => t.AssignedToUserId == myUserId).Count();
                    break;
                default:
                    break;
            }
         
            if(ticketCnt == 0)
            {
                return "0%";
            }
           
            decimal statusCount = GetTicketCountByStatus(status);
            decimal placement = statusCount / ticketCnt * 100;
            //return $"{placement.ToString()}%";
            return placement.ToString() + "%";

        }

        public static int GetTicketCountByPriority(string priority)
        {
            var myUserId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(myUserId).FirstOrDefault();
            var ticketCnt = 0;
            switch (myRole)
            {
                case "Admin":
                    ticketCnt = db.Tickets.Where(t => t.TicketPriority.Name == priority).Count();
                    break;
                case "PM":
                    var myProjects = projHelper.ListUserProjects(myUserId);
                    foreach (var project in myProjects)
                    {
                        foreach (var ticket in project.Tickets)
                        {
                            if (ticket.TicketPriority.Name == priority)
                            {
                                ticketCnt += 1;
                            }
                        }
                    }
                    break;
                case "Sub":
                    ticketCnt = db.Tickets.Where(t => t.OwnerUserId == myUserId && t.TicketPriority.Name == priority).Count();
                    break;
                case "Devs":
                    ticketCnt = db.Tickets.Where(t => t.AssignedToUserId == myUserId && t.TicketPriority.Name == priority).Count();
                    break;
                default:
                    break;
            }
            return ticketCnt;

        }

        public static string GetTicketPlacementByPriority(string priority)
        {
            decimal ticketCnt = 0;
            var myUserId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(myUserId).FirstOrDefault();
            switch (myRole)
            {
                case "Admin":
                    ticketCnt = db.Tickets.Count();
                    break;
                case "PM":
                    var myProjects = projHelper.ListUserProjects(myUserId);
                    foreach (var project in myProjects)
                    {
                        foreach (var ticket in project.Tickets)
                        {
                            ticketCnt += 1;
                        }
                    }
                    break;
                case "Sub":
                    ticketCnt = db.Tickets.Where(t => t.OwnerUserId == myUserId).Count();
                    break;
                case "Devs":
                    ticketCnt = db.Tickets.Where(t => t.AssignedToUserId == myUserId).Count();
                    break;
                default:
                    break;
            }

            if (ticketCnt == 0)
            {
                return "0%";
            }

            decimal statusCount = GetTicketCountByPriority(priority);
            decimal placement = statusCount / ticketCnt * 100;
            //return $"{placement.ToString()}%";
            return placement.ToString() + "%";

        }

    }
}