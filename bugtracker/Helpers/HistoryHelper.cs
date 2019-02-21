using bugtracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace bugtracker.Helpers
{
    public class HistoryHelper
    {
        public void AddHistory(Ticket oldTicket, Ticket newTicket)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            TicketHistory ticketHistory;


            if (oldTicket.Title != newTicket.Title)
            {
                //We need to call Createhistory and add a new history record
                ticketHistory = Createhistory("Title", oldTicket.Title, newTicket.Title, newTicket.Id);
                db.TicketHistories.Add(ticketHistory);
            }
            if (oldTicket.Description != newTicket.Description)
            {
                ticketHistory = Createhistory("Description", oldTicket.Description, newTicket.Description, newTicket.Id);
                db.TicketHistories.Add(ticketHistory);
            }
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                ticketHistory = Createhistory("TicketStatusId", oldTicket.TicketStatusId.ToString(), newTicket.TicketStatusId.ToString(), newTicket.Id);
                db.TicketHistories.Add(ticketHistory);
            }
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                ticketHistory = Createhistory("TicketPriorityId", oldTicket.TicketPriorityId.ToString(), newTicket.TicketPriorityId.ToString(), newTicket.Id);
                db.TicketHistories.Add(ticketHistory);
            }
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                ticketHistory = Createhistory("TicketTypeId", oldTicket.TicketTypeId.ToString(), newTicket.TicketTypeId.ToString(), newTicket.Id);
                db.TicketHistories.Add(ticketHistory);
            }
            if (oldTicket.AssignedToUserId != newTicket.AssignedToUserId)
            {
                ticketHistory = Createhistory("AssignedToUserId", oldTicket.AssignedToUserId, newTicket.AssignedToUserId, newTicket.Id);
                db.TicketHistories.Add(ticketHistory);
            }
            db.SaveChanges();

        }



        public TicketHistory Createhistory(string propertyName, string oldValue, string newValue, int ticketId)
        {
            var history = new TicketHistory()
            {
                TicketId = ticketId,
                Changed = DateTime.Now,
                NewValue = newValue,
                OldValue = oldValue,
                PropertyName = propertyName,
                UserId = HttpContext.Current.User.Identity.GetUserId()
            };
            return history;
        }


        private static ApplicationDbContext dbStatic = new ApplicationDbContext();

        public static string ConvertToName(string propertyName, string Id)
        {
            var name = "";
            if (propertyName == "TicketStatusId")
            {
                name = dbStatic.TicketStatuses.Find(Convert.ToInt32(Id)).Name;
            }
            if (propertyName == "TicketTypeId")
            {
                name = dbStatic.TicketTypes.Find(Convert.ToInt32(Id)).Name;
            }
            if (propertyName == "TicketPriorityId")
            {
                name = dbStatic.TicketPriorities.Find(Convert.ToInt32(Id)).Name;
            }
            if (propertyName == "AssignedToUserId")
            {
                name = dbStatic.Users.Find(Id).FirstName;
            }


            return name;
        }

    }

}