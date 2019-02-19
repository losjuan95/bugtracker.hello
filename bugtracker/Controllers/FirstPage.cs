using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bugtracker.Controllers
{
    public class FirstPage : Controller
    {
        // GET: FirstPage
        public ActionResult Index()
        {
            return View();
        }

        // GET: FirstPage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FirstPage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FirstPage/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FirstPage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FirstPage/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FirstPage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FirstPage/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
