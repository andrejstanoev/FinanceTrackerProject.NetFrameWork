using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinanceTrackerProject.Models;
using Microsoft.AspNet.Identity;

namespace FinanceTrackerProject.Controllers
{
    [Authorize]
    public class WelcomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Welcome
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var appUser = db.Users.FirstOrDefault(u=>u.Id == userId);


            UserViewModel model = new UserViewModel()
            {
                UserName = appUser.UserName,
                Email = appUser.Email,
                NumBudgets = db.Budgets.Count(b=>b.UserId == userId),
                NumExpenses = db.Expenses.Count(e => e.UserId == userId),
                TotalBudget = db.Budgets.Where(b => b.UserId == userId).Sum(b => (float?)b.Ammount) ?? 0,
                LatestBudgets = (db.Budgets.Where(b => b.UserId == userId))==null ? new List<Budget>() : db.Budgets.Where(b => b.UserId == userId)
                              .OrderByDescending(b => b.Date)
                              .Take(3)
                              .ToList()
            };
            return View(model);
        }
    }
}