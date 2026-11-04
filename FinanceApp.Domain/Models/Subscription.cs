using FinanceApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public BillingCycle BillingCycle { get; set; } = BillingCycle.Monthly;
        public DateTime NextBillingDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int? AccountId { get; set; }
        public Account? Account { get; set; }

        public void AdvanceNextBillingDate()
        {
            NextBillingDate = BillingCycle switch
            {
                BillingCycle.Weekly => NextBillingDate.AddDays(7),
                BillingCycle.BiWeekly => NextBillingDate.AddDays(14),
                BillingCycle.Every28Days => NextBillingDate.AddDays(28),
                BillingCycle.Monthly => NextBillingDate.AddMonths(1),
                BillingCycle.Annually => NextBillingDate.AddYears(1),
                _ => NextBillingDate.AddMonths(1)
            };
        }
    }
}
