﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bugtracker.Models
{
    public class TicketPriority
    {
        public int Id { get; set; }
        public string Name { get; set; }

       


        public virtual ICollection<Ticket>Tickets { get; set; }

        public TicketPriority()
        {
            this.Tickets = new HashSet<Ticket>();
        }
    }
}