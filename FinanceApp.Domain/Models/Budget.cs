using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal AmountLimit { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
