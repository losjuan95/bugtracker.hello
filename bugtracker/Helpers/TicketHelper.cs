﻿using bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bugtracker.Helpers
{
    public class TicketHelper
    {
       static  ApplicationDbContext db = new ApplicationDbContext();

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

    }
}