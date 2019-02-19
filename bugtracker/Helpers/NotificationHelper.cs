using bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Mail;

namespace bugtracker.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void Notify(Ticket oldTicket, Ticket newTicket)
        {
            // The purpose of this method is to determine whether or not the system needs to generate TicketNotification records
            var oldUserId = oldTicket.AssignedToUserId;
            var newUserId = newTicket.AssignedToUserId;

            //If the oldUserId and the newUserId and the same there is nothing to do...
            if (oldUserId == newUserId)
                return;

            //If I am here...that means I am generating a Notification
            var newNotification = new TicketNotification
            {
                Created = DateTime.Now,
                TicketId = newTicket.Id
            };

            // Condition 1: The Ticket is newly assigned. This means that the oldTicket has an AssignedToUserId = null and the newTicket has someone assigned
            if (oldUserId == null && newUserId != null)
            {
                //This condition needs to trigger an Assignment Notification record
                newNotification.RecipientId = newUserId;
                newNotification.NoticationBody = $"You have been assigned to Ticket {newTicket.Id}";
                db.TicketNotifications.Add(newNotification);
                db.SaveChanges();
            }

            //Condition 2: The Ticket has been newly unassigned. This means that the OldTicket had a AssignedToUserId and the NewTicket does not
            else if (oldUserId != null && newUserId == null)
            {
                //This condition needs to trigger an UnAssignment Notification record
                newNotification.RecipientId = oldUserId;
                newNotification.NoticationBody = $"You have been unassigned from Ticket {newTicket.Id}";
                db.TicketNotifications.Add(newNotification);
                db.SaveChanges();
            }

            //Condition 3: Neither the OldTicket nor the NewTicket AssignedToUserId's are null and they are different. This means the Ticket was reassigned
            else
            {
                //This condition needs to trigger both an UnAssignment and an Assignment Notification record
                newNotification.RecipientId = newUserId;
                newNotification.NoticationBody = $"You have been assigned to Ticket {newTicket.Id}";
                db.TicketNotifications.Add(newNotification);

                var secondNotification = new TicketNotification
                {
                    Created = DateTime.Now,
                    TicketId = newTicket.Id,
                    RecipientId = oldUserId,
                    NoticationBody = $"You have been unassigned from Ticket {newTicket.Id}"
                };

                db.TicketNotifications.Add(secondNotification);
                db.SaveChanges();
            }
        }
    }
}