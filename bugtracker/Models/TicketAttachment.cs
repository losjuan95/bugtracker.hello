using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace bugtracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }

        public int TicketId { get; set; }
        public string UserId { get; set; }

        [MaxLength(40), MinLength(1)]
        public string Description { get; set; }
        public string FilePath { get; set; }
        public DateTime Created { get; set; }
        
       

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }




    }
}