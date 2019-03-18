using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace bugtracker.Models
{
    public class Project
    {
        public int Id { get; set; }

        [MaxLength(30), MinLength(1)]
        public string Name { get; set; }

        [MaxLength(40), MinLength(1)]
        public string Description { get; set; }

       public virtual  ICollection<Ticket>Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Project()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.Tickets = new HashSet<Ticket>();
        }
    }
   
}