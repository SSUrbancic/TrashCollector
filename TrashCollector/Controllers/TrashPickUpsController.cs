using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrashCollector.Models;

namespace TrashCollector.Controllers
{
    public class TrashPickUpsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TrashPickUps
        public ActionResult Index()
        {
            return View(db.TrashPickUps.ToList());
        }
        public ViewResult EmployeePickUps(string day)
        {
            DateTime today = DateTime.Today;
            string d = today.DayOfWeek.ToString();

            List<string> daysOfTheWeek = new List<string>() { "Monday", "Tuesday", "Wendsday", "Thursday", "Friday", "Saturday", "Sunday" };
            ViewBag.days = new SelectList(daysOfTheWeek);
            string userID = User.Identity.GetUserId();
            Employee employee = db.Employees.Where(x => x.userID == userID).Select(x => x).First();
            var employeePickUps = db.TrashPickUps.Where(x => x.Customer.zipCode == employee.AssignedZipCode).Where(x => x.dayOfWeek == d);
            if(day != null)
            {
                employeePickUps = db.TrashPickUps.Where(x => x.Customer.zipCode == employee.AssignedZipCode).Where(x => x.dayOfWeek == day);
            }
            return View(employeePickUps.ToList());
        }
        public ViewResult CustomerPickUps(string day)
        {
            List<string> daysOfTheWeek = new List<string>() { "Monday", "Tuesday", "Wendsday", "Thursday", "Friday", "Saturday", "Sunday" };
            ViewBag.days = new SelectList(daysOfTheWeek);
            string userID = User.Identity.GetUserId();
            Customer customer = db.Customers.Where(x => x.userID == userID).Select(x => x).First();
            var customerPickUps = db.TrashPickUps.Where(x => x.Customer.ID == customer.ID);
            if (day != null)
            {
                customerPickUps = db.TrashPickUps.Where(x => x.Customer.zipCode == customer.zipCode).Where(x => x.dayOfWeek == day);
            }
            return View(customerPickUps.ToList());
        }

        // GET: TrashPickUps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrashPickUp trashPickUp = db.TrashPickUps.Find(id);
            if (trashPickUp == null)
            {
                return HttpNotFound();
            }
            return View(trashPickUp);
        }

        // GET: TrashPickUps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrashPickUps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pickUpID,date,dayOfWeek")] TrashPickUp trashPickUp)
        {
            string userID = User.Identity.GetUserId();
            Customer customer = db.Customers.Where(x => x.userID == userID).Select(x => x).First();
            if (ModelState.IsValid)
            {
                trashPickUp.CustomerID = customer.ID;
                db.TrashPickUps.Add(trashPickUp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trashPickUp);
        }

        // GET: TrashPickUps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrashPickUp trashPickUp = db.TrashPickUps.Find(id);
            if (trashPickUp == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName", trashPickUp.CustomerID);
            return View(trashPickUp);
        }

        // POST: TrashPickUps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pickUpID,date,dayOfWeek,pickUpCompleted,price")] TrashPickUp trashPickUp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trashPickUp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trashPickUp);
        }

        // GET: TrashPickUps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrashPickUp trashPickUp = db.TrashPickUps.Find(id);
            if (trashPickUp == null)
            {
                return HttpNotFound();
            }
            return View(trashPickUp);
        }

        // POST: TrashPickUps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrashPickUp trashPickUp = db.TrashPickUps.Find(id);
            db.TrashPickUps.Remove(trashPickUp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
