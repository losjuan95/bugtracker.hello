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

        //public TicketHistory Convert(string propertyName, int Id)
        //{
        //    var tickethistory = new TicketHistory();
        //    var name = new propertyName.convert
        //    foreach(var tickethistory in id)
        //    {

        //    }
        //    return Convert();
        //}

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
        
    }

}