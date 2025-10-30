using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinanceTrackerProject.Models
{
	public class ExpenseViewModel
	{
        public string Name { get; set; }
        public float Ammount { get; set; }
        public int BudgetId { get; set; }
    }
}