using bugtracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bugtracker.Helpers
{
  
        public class UserHelper
        {
            private static ApplicationDbContext db = new ApplicationDbContext();
            public static string GetFirstName()
            {
                var userId = HttpContext.Current.User.Identity.GetUserId();
                return db.Users.Find(userId).FirstName;
            }
        }
    
}