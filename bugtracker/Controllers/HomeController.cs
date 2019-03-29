using bugtracker.Helpers;
using bugtracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace bugtracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
           return View(db.TicketHistories.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            Email model= new Email();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAvatar(HttpPostedFileBase image)
        {

            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if (FileUploadValidator.AvatarUploadValidator(image))
            {

                var imageName = Path.GetFileName(image.FileName);
                image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), imageName));
                user.AvatarPath = "/Uploads/" + imageName;
                db.Users.Attach(user);
                db.Entry(user).Property(u => u.AvatarPath).IsModified = true;
                db.SaveChanges();

                return RedirectToAction("Index", "Manage");

            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }


        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(Email model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From:<bold>{0}({1})</p><p>Message:</p><p>{2}</p>";
                    var from = "BugTracker<anyemailher@host.com>";

                    model.Body = "this is a message from your BugTracker. The name and the email of the contacting person is above." + model.Body;
                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = model.Subject,
                        Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);

                    return View(new Email());

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);

                }
            }
            return View(model);

        }

    }

    

}