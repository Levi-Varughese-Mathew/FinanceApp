using System;
using System.Collections.Generic;
using System.Text;
using FinanceApp.Domain.Models;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Services
{
    public class TransactionService
    {
        // Getting the db context from the DI container and using it to perform CRUD operations on the Account entity.
        private readonly FinanceDbContext _context;

        // Declaring the constructor for the TransactionService class, which takes a FinanceDbContext as a parameter and assigns it to the _context field.
        public TransactionService(FinanceDbContext context)
        {
            _context = context;
        }

        // Getting all the transactions
        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .Include(t => t.Account) // To get the account details associated with each transaction
                .Include(t => t.Category) // To get the category details associated with each transaction
                .ToListAsync();
        }

        // Getting filtered transactions based
        public async Task<List<Transaction>> GetFilteredTransactionsAsync(int? accountId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Transactions.AsQueryable();
            if (accountId.HasValue)
            {
                query = query.Where(t => t.AccountId == accountId.Value);
            }
            if (startDate.HasValue)
            {
                query = query.Where(t => t.Date >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(t => t.Date <= endDate.Value);
            }
            return await query
                //.AsNoTracking() // To improve performance since we don't need to track changes for read-only operations
                .Include(t => t.Account) // To get the account details associated with each transaction
                .Include(t => t.Category) // To get the category details associated with each transaction
                .ToListAsync();
        }

        // Add a new transaction (logic for different accounts and transaction types is handled in the controller)
        public async Task AddTransactionAsync(Transaction transaction)
        {
            // Reducing the amount from the account balance if it is an expense transaction, and adding it to the account balance if it is an income transaction
            if (transaction.Type == Domain.Enums.TransactionType.Expense)
            {
                var account = await _context.Accounts.FindAsync(transaction.AccountId);
                if (account != null)
                {
                    // Reducing the amount from the balance (this is valid for checking, savings, and credit card accounts, but not for investment accounts)
                    account.Balance -= transaction.Amount;
                    _context.Accounts.Update(account); // Marking the account entity as modified to ensure the changes are saved to the database
                }
            }
            else if (transaction.Type == Domain.Enums.TransactionType.Income)
            {
                var account = await _context.Accounts.FindAsync(transaction.AccountId);
                if (account != null)
                {
                    account.Balance += transaction.Amount;
                    _context.Accounts.Update(account); // Marking the account entity as modified to ensure the changes are saved to the database
                }
            }

            // Adding the transaction to the database and saving changes
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        // Update an existing transaction
        public async Task UpdateTransactionAsync(Transaction transaction)
        {

            // Getting the transaction from the database to avoid tracking issues when updating an entity that is already being tracked by the context.
            var selectedTranaction = await _context.Transactions.FindAsync(transaction.Id);

            // Updating the account balance based on the difference between the old and new transaction amounts, and the transaction type (expense or income).
            if (selectedTranaction != null)
            {
                // Getting the account associated with the transaction to update its balance based on the transaction type and amount.
                var account = await _context.Accounts.FindAsync(selectedTranaction.AccountId);
                if (account != null)
                {
                    // If it is an expense, the old expense amount is added back to the balance of the related account, then the new expense is deducted from the balance.
                    if (selectedTranaction.Type == Domain.Enums.TransactionType.Expense)
                    {
                        account.Balance += selectedTranaction.Amount; // Reverting the old expense amount
                        account.Balance -= transaction.Amount; // Applying the new expense amount
                    }
                    //If it is an income, the old income amount is subtracted from the balance of the related account, then the new income is added to the balance
                    else if (selectedTranaction.Type == Domain.Enums.TransactionType.Income)
                    {
                        account.Balance -= selectedTranaction.Amount; // Reverting the old income amount
                        account.Balance += transaction.Amount; // Applying the new income amount
                    }
                    _context.Accounts.Update(account); // Marking the account entity as modified to ensure the changes are saved to the database
                }
            }

            // Updating values individually to avoid tracking issues when updating an entity that is already being tracked by the context.
            if (selectedTranaction != null)
            {
                selectedTranaction.Amount = transaction.Amount;
                selectedTranaction.Date = transaction.Date;
                selectedTranaction.AccountId = transaction.AccountId;
                //selectedTranaction.Account = transaction.Account;
                //selectedTranaction.Category = transaction.Category;
                selectedTranaction.CategoryId = transaction.CategoryId;
                selectedTranaction.Notes = transaction.Notes;
                selectedTranaction.Type = transaction.Type;
                selectedTranaction.Description = transaction.Description;
            }

            // This will mark the transaction entity as modified to ensure the changes are saved to the database.
            // If there is an issue with any of the logic above, this will throw an exception and the changes will not be saved to the database.
            await _context.SaveChangesAsync();
        }

        // Delete a transaction
        public async Task DeleteTransactionAsync(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);

            // Updating the account balance based on the transaction type and amount before deleting the transaction.
            if (transaction != null)
            {
                var account = await _context.Accounts.FindAsync(transaction.AccountId);
                if (account != null)
                {
                    if (transaction.Type == Domain.Enums.TransactionType.Expense)
                    {
                        account.Balance += transaction.Amount; // Reverting the expense amount
                    }
                    else if (transaction.Type == Domain.Enums.TransactionType.Income)
                    {
                        account.Balance -= transaction.Amount; // Reverting the income amount
                    }
                    _context.Accounts.Update(account); // Marking the account entity as modified to ensure the changes are saved to the database
                }

                // Removing the transaction from the database and saving changes
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }            
        }


        // Getting the expenses for the month of a specific year (month and year are passed as parameters)
        // Default values for month and year are set to the current month and year if not provided.
        public async Task<List<Transaction>> GetExpensesForMonthAsync(int? month = null, int? year = null)
        {
            int m = month ?? DateTime.Now.Month;
            int y = year ?? DateTime.Now.Year;

            var startDate = new DateTime(y, m, 1);
            var endDate = startDate.AddMonths(1); // First day of the *next* month

            return await _context.Transactions
                //.AsNoTracking() // To improve performance since we don't need to track changes for read-only operations
                .Include(t => t.Account) // To get the account details associated with each transaction
                .Include(t => t.Category) // To get the category details associated with each transaction
                .Where(t => t.Date >= startDate && t.Date < endDate)
                .Where(t => t.Type == Domain.Enums.TransactionType.Expense)
                .OrderByDescending(t => t.Date) // <-- Sorts by date (newest first)
                .ToListAsync();
        }

        // Getting income for the specified month and year (defaulting to the current month and year if not provided)
        public async Task<List<Transaction>> GetIncomeForMonthAsync(int? month = null, int? year = null)
        {
            int m = month ?? DateTime.Now.Month;
            int y = year ?? DateTime.Now.Year;

            var startDate = new DateTime(y, m, 1);
            var endDate = startDate.AddMonths(1); // First day of the *next* month

            return await _context.Transactions
                .AsNoTracking() // To improve performance since we don't need to track changes for read-only operations
                .Include(t => t.Account) // To get the account details associated with each transaction
                .Include(t => t.Category) // To get the category details associated with each transaction
                .Where(t => t.Date >= startDate && t.Date < endDate)
                .Where(t => t.Type == Domain.Enums.TransactionType.Income)
                .ToListAsync();
        }
    }
}
