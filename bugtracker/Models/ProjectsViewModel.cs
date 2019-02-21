using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bugtracker.Models
{
    public class ProjectsViewModel
    {
       
            public List<Project> AllProjects { get; set; }

            public List<Project> DevProjects { get; set; }

            public List<Project> SubProjects { get; set; }

           public List<Project> PMProjects { get; set; }

            public ProjectsViewModel()
            {
                AllProjects = new List<Project>();

                DevProjects = new List<Project>();

                SubProjects = new List<Project>();

                PMProjects = new List<Project>();
            }

        
    }
}