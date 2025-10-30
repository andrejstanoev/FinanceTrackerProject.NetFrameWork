using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinanceTrackerProject.Models
{
	public class EditExpenseViewModel
	{
        public int budgetId { get; set; }
        public int expenseId { get; set; }

        public string Name { get; set; }

        public float Ammount { get; set; }
    }
}