using FinanceApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public TransactionType Type { get; set; }
        public int? AccountId { get; set; }
        public Account? Account { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
