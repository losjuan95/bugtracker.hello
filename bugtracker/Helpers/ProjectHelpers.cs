﻿using bugtracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
//using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace bugtracker.Helpers
{
    public class ProjectHelpers
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var flag = project.Users.Any(u => u.Id == userId);
            return (flag);
        }
        public ICollection<Project>ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);

            var projects = user.Projects.ToList();
            return (projects);
        }

        public void AddUserToProject(string userId, int projectId)
        {
            if(!IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);

                proj.Users.Add(newUser);
                db.SaveChanges();
                    
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if(IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var delUser = db.Users.Find(userId);

                proj.Users.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified; //just saves this obj instance.
                db.SaveChanges();
              
            }
        }

        public ICollection<ApplicationUser> UsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users.ToList();
        }


        public ICollection<string> GetProjectUserRoles(string roleName, int projectId)
        {
            var users = UsersOnProject(projectId);
            var usersInRole = new List<string>();
            var roleHelper = new UserRolesHelpers();

            foreach (var user in users)
            {
                if (roleHelper.IsUserInRole(user.Id, roleName))
                {
                    usersInRole.Add(user.Email);
                }
            }
            return usersInRole;
        }
        public ICollection<string> GetProjectUserRolesName(string roleName, int projectId)
        {
            var users = UsersOnProject(projectId);
            var usersInRole = new List<string>();
            var roleHelper = new UserRolesHelpers();

            foreach (var user in users)
            {
                if (roleHelper.IsUserInRole(user.Id, roleName))
                {
                    usersInRole.Add(user.FirstName);
                }
            }
            return usersInRole;
        }

    }
}