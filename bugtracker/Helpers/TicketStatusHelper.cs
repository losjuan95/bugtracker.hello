using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bugtracker.Helpers
{
    public class TicketStatusHelper
    {
        public static string GetStatusClass(string status)
        {
            var myclass = "";

            switch (status)
            {

                case "Open":
                    myclass = "label-primary";
                    break;
                case "On-Hold":
                    myclass = "label-danger";
                    break;
                case "Closed":
                    myclass = "label-success";
                    break;
                default:
                    myclass = "label-info";
                    break;
            }


            return myclass;
        }
    }
}