﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bugtracker.Models
{
    public class TicketType
    {
        public int Id { get; set; }
        public string Name { get; set; }


        //public virtual Ticket Ticket { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

        public TicketType()
        {
            this.Tickets = new HashSet<Ticket>();
        }
        
    }
}