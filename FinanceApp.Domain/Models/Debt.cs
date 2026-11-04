using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Models
{
    public class Debt
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal InitialAmount { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal InterestRate { get; set; }
        public decimal MinimumMonthlyPayment { get; set; }
        public int DueDayOfMonth { get; set; }
        public bool IsActive { get; set; } = true;
        public int? AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
