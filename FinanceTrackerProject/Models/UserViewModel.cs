using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinanceTrackerProject.Models
{
	public class UserViewModel
	{
        public string UserName { get; set; }
        public string Email { get; set; }
        public int NumBudgets { get; set; }
        public int NumExpenses { get; set; }
        public float TotalBudget { get; set; }
        public List<Budget> LatestBudgets { get; set; }

        public UserViewModel() { 
            LatestBudgets = new List<Budget>();
        }  
    }
}