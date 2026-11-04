using System;
using System.Collections.Generic;
using System.Text;
using FinanceApp.Domain.Enums;

namespace FinanceApp.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TransactionType TransactionType { get; set; }
        public List<Transaction> Transactions { get; set; } = new();
    }
}
