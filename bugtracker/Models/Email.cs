﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace bugtracker.Models
{
    public class Email
    {
        [Required, Display(Name = "Name")]
        public string FromName { get; set; }

        [Required, EmailAddress]
        public string ToEmail { get; set; }

        [Required, Display(Name = "Email"), EmailAddress]
        public string FromEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

    }
}