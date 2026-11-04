using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Enums
{
    // Represents the billing cycle for recurring payments or subscriptions.
    public enum BillingCycle
    {
        Weekly = 7,
        BiWeekly = 14,
        Every28Days = 28,
        Monthly = 30,
        Annually = 365
    }
}
