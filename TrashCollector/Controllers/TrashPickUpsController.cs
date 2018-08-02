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
using GoogleMaps.LocationServices;

namespace TrashCollector.Controllers
{
    public class TrashPickUpsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TrashPickUps
        public ActionResult GoogleMaps(int id)
        {
            Customer customer = db.Customers.Where(x => x.ID == id).Select(x => x).First();
            string addressLineOneFormated = customer.addressLine1.Replace(' ', '+');
            string addressLineTwoFormated = (!string.IsNullOrEmpty(customer.addessLine2)) ? customer.addessLine2.Replace(' ', '+') : null;
            string streetAddress = (!string.IsNullOrEmpty(addressLineTwoFormated)) ? addressLineOneFormated + "+" + addressLineTwoFormated : addressLineOneFormated;
            string formattedCity = customer.city.Replace(' ', '+');
            string formattedAddressComplete = streetAddress + "," + formattedCity + "," + customer.state + "," + customer.zipCode.ToString();
            ViewBag.Address = formattedAddressComplete;

            var testAddress = customer.addressLine1 + " " + ((!string.IsNullOrEmpty(customer.addessLine2)) ? customer.addessLine2 + " " : "") + customer.city + ", " + customer.state + " " + customer.zipCode;
            var locationService = new GoogleLocationService();
            var point = locationService.GetLatLongFromAddress(testAddress);
            var latitude = point.Latitude;
            var longitude = point.Longitude;
            ViewBag.Point = point;
            ViewBag.Lat = latitude;
            ViewBag.Long = longitude;
            var trashPickUp = db.TrashPickUps.Where(x => x.CustomerID == id).Include(x => x.Customer).First();
            return View(trashPickUp);
        }
        public ActionResult Index()
        {
            return View(db.TrashPickUps.ToList());
        }
        public ViewResult EmployeePickUps(string day)
        {
            DateTime today = DateTime.Today;
            string d = today.DayOfWeek.ToString();

            List<string> daysOfTheWeek = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
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
        public ViewResult CustomerEdit()
        {
            List<string> stuff = new List<string>() { };
            return View();
        }
        public ActionResult EmployeeViewForPickUpDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var trashPickUp = db.TrashPickUps.Where(x => x.pickUpID == id).Include(x => x.Customer).First();           

            if (trashPickUp == null)
            {
                return HttpNotFound();
            }
            return View(trashPickUp);
        }
        public ViewResult CustomerPickUps(string day)
        {
            List<string> daysOfTheWeek = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
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
                if(User.IsInRole("Customer"))
                {
                    return RedirectToAction("CustomerPickUps");
                }
                else
                {
                    return RedirectToAction("EmployeePickUps");
                }
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
        public ActionResult Edit([Bind(Include = "pickUpID,date,dayOfWeek,pickUpCompleted,price,CustomerID")] TrashPickUp trashPickUp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trashPickUp).State = EntityState.Modified;
                
                if(trashPickUp.pickUpCompleted == true && User.IsInRole("Employee"))
                {

                    var transactionAmount = trashPickUp.price;
                    var customerBalance = db.TrashPickUps.Where(x => x.CustomerID == trashPickUp.CustomerID).Select(x => x.Customer.paymentBalance).First();
                    var newCustomerBalance = transactionAmount + customerBalance;
                    var customer = db.TrashPickUps.Where(x => x.pickUpID == trashPickUp.pickUpID).Select(x => x.Customer).First();
                    customer.paymentBalance = newCustomerBalance;
                }
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
