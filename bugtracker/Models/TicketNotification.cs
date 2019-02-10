using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bugtracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }
        public int TicketId{ get; set; }
        //public int UserId { get; set; }
        //public string SenderId{ get; set; }
        public string RecipientId { get; set; }


        public string NoticationBody { get; set; }
        public DateTime Created { get; set; }
        public bool Read { get; set; }

        public bool Confirmed { get; set; }


        public virtual Ticket Ticket { get; set; }
        //public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Recipient { get; set; }

    }
}