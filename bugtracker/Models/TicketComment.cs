using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace bugtracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string CommentTitle { get; set; }
        
        public string CommentBody { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public string UpdateReason { get; set; }

        public string UserId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get;set;}
    }
}