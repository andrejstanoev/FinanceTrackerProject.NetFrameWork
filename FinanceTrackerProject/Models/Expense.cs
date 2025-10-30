using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinanceTrackerProject.Models
{
	public class Expense
	{
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public float Ammount { get; set; }

        public int BudgetId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Budget Budget { get; set; }

        public Expense()
        {
            Date = DateTime.Now;
        }
    }
}