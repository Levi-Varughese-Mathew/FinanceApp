using System;
using System.Collections.Generic;
using System.Text;
using FinanceApp.Domain.Enums;

namespace FinanceApp.Domain.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public AccountType AccountType { get; set; }

        // Navigation property for related transactions
        public List<Transaction> Transactions { get; set; } = new();
    }
}
