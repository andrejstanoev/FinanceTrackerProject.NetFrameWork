using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinanceTrackerProject.Models;
using Microsoft.AspNet.Identity;

namespace FinanceTrackerProject.Controllers
{
    [Authorize]
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Budgets
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();

            var budgets = db.Budgets.Where(b=>b.UserId == currentUserId).Include(b => b.User).Include(b => b.Expenses);

            return View(budgets.ToList());
        }

        // GET: Budgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var budget = db.Budgets
               .Include(b => b.Expenses)
               .Include(b => b.User)
               .FirstOrDefault(b => b.Id == id);

            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // GET: Budgets/Create
        public ActionResult Create()
        {
            
            return View();
        }

        //// POST: Budgets/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BudgetViewModel model)
        {
            Budget budget = new Budget()
            {
                Name = model.Name,
                Ammount = model.Ammount,
                UserId = User.Identity.GetUserId()
            };
            if (ModelState.IsValid)
            {
                db.Budgets.Add(budget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", budget.UserId);
            return View(budget);
        }

        //// GET: Budgets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", budget.UserId);
            return View(budget);
        }

        //// POST: Budgets/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Ammount,UserId")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                Budget currentBudget = db.Budgets.Include(b=>b.Expenses).FirstOrDefault(b=>b.Id == budget.Id);
                currentBudget.Name = budget.Name; 
                currentBudget.Ammount = budget.Ammount;
                
                db.SaveChanges();
                return RedirectToAction("Details", new { id = budget.Id });
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", budget.UserId);
            return View(budget);
        }

        

       
        public ActionResult Delete(int id)
        {
            Budget budget = db.Budgets.Find(id);
            db.Budgets.Remove(budget);
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
