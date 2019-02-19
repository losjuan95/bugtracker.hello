using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bugtracker.Helpers;

namespace bugtracker.Models
{
    public class TicketsViewModel
    {
        public List<Ticket> AllTickets { get; set; }

        public List<Ticket> DevTickets { get; set; }

        public List<Ticket> SubTickets { get; set; }

        public List<Ticket> ProjTickets { get; set; }

        public TicketsViewModel()
        {
            AllTickets = new List<Ticket>();

            DevTickets = new List<Ticket>();

            SubTickets = new List<Ticket>();

            ProjTickets = new List<Ticket>();
        }

    }
}