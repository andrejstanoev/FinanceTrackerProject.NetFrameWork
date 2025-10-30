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
    public class ExpensesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Expenses
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var expenses2 = db.Expenses.Where(e => e.UserId == userId).Include(e => e.Budget);

            //var expenses = db.Expenses.Include(e => e.Budget).Include(e => e.User);
            return View(expenses2.ToList());
        }

        // GET: Expenses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        public ActionResult CreateForBudget(int? id)
        {
            ViewBag.budgetId=id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateForBudget( ExpenseViewModel model)
        {
            Budget budget = db.Budgets.Include(b=>b.Expenses).Where(b=>b.Id == model.BudgetId).FirstOrDefault();
            var spent = budget.Expenses?.Sum(e => (float?)e.Ammount) ?? 0;
            var remaining = budget.Ammount - spent;
            if(model.Ammount > remaining)
            {
                ModelState.AddModelError("Ammount", "The amount exceeds the remaining budget ($" + remaining.ToString("N2") + ").");
                ViewBag.budgetId = model.BudgetId;
                return View(model);
            }
            Expense newExpense = new Expense()
            {
                Name = model.Name,
                Ammount = model.Ammount,
                BudgetId = budget.Id,
                Budget = budget,
                UserId = User.Identity.GetUserId(),

            };
            db.Expenses.Add(newExpense);
            budget.Expenses.Add(newExpense);
            db.SaveChanges();
            return RedirectToAction("Details", "Budgets",new { id = budget.Id });
        }

        // GET: Expenses/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.BudgetId = new SelectList(db.Budgets.Where(b=>b.UserId== userId), "Id", "Name");
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Ammount,BudgetId,Date,UserId")] Expense expense)
        {
            Budget budget = db.Budgets.Include(b => b.Expenses).Where(b => b.Id == expense.BudgetId).FirstOrDefault();
            var spent = budget.Expenses?.Sum(e => (float?)e.Ammount) ?? 0;
            var remaining = budget.Ammount - spent;
            if (expense.Ammount > remaining)
            {
                var userId = User.Identity.GetUserId();
                ModelState.AddModelError("Ammount", "The amount exceeds the remaining budget ($" + remaining.ToString("N2") + ").");
                ViewBag.BudgetId = new SelectList(db.Budgets.Where(b => b.UserId == userId), "Id", "Name");
                return View(expense);
            }

            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var newExpense = new Expense() { 
                    Name = expense.Name,
                    Ammount= expense.Ammount,
                    BudgetId= expense.BudgetId,
                    UserId= userId,
                };
                db.Expenses.Add(newExpense);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name", expense.BudgetId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", expense.UserId);
            return View(expense);
        }

        // GET: Expenses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name", expense.BudgetId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", expense.UserId);
            return View(expense);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Ammount,BudgetId,Date,UserId")] Expense expense)
        {
            Budget budget = db.Budgets.Include(b => b.Expenses).Where(b => b.Id == expense.BudgetId).FirstOrDefault();
            var spent = budget.Expenses?.Sum(e => (float?)e.Ammount) ?? 0;
            var remaining = budget.Ammount - spent;
            if (expense.Ammount > remaining)
            {
                ModelState.AddModelError("Ammount", "The amount exceeds the remaining budget ($" + remaining.ToString("N2") + ").");
                ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name", expense.BudgetId);
                return View(expense);
            }

            if (ModelState.IsValid)
            {
                Expense currentExpense = db.Expenses.Include(e=>e.Budget).FirstOrDefault(e=>e.Id == expense.Id);
                currentExpense.Name = expense.Name;
                currentExpense.Ammount = expense.Ammount;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name", expense.BudgetId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", expense.UserId);
            return View(expense);
        }

        public ActionResult EditWithinBudget(int? budgetId, int? expenseId)
        {
            var budget = db.Budgets.Where(b => b.Id == budgetId).FirstOrDefault();
            var expense = db.Expenses.Where(e=>e.Id==expenseId).FirstOrDefault();

            EditExpenseViewModel model = new EditExpenseViewModel()
            {
                budgetId = budget.Id,
                expenseId = expense.Id,
                Name = expense.Name,
                Ammount = expense.Ammount,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWithinBudget(EditExpenseViewModel model)
        {
            Budget budget = db.Budgets.Include(b => b.Expenses).Where(b => b.Id == model.budgetId).FirstOrDefault();
            var spent = budget.Expenses?.Sum(e => (float?)e.Ammount) ?? 0;
            var remaining = budget.Ammount - spent;
            if (model.Ammount > remaining)
            {
                ModelState.AddModelError("Ammount", "The amount exceeds the remaining budget ($" + remaining.ToString("N2") + ").");
                ViewBag.budgetId = model.budgetId;
                return View(model);
            }
            if (ModelState.IsValid)
            {
                Expense currentExpense = db.Expenses.Include(e=>e.Budget).FirstOrDefault(e=>e.Id == model.expenseId);
                currentExpense.Name = model.Name;
                currentExpense.Ammount = model.Ammount;
                db.SaveChanges();

                return RedirectToAction("Details", "Budgets", new { id = model.budgetId });
            }
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            Expense expense = db.Expenses.Find(id);
            db.Expenses.Remove(expense);
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
