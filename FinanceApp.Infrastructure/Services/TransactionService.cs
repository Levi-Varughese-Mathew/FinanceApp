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
                .Include(t => t.Account) // To get the account details associated with each transaction
                .Include(t => t.Category) // To get the category details associated with each transaction
                .ToListAsync();
        }

        // Add a new transaction
        public async Task AddTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        // Update an existing transaction
        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        // Delete a transaction
        public async Task DeleteTransactionAsync(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction != null)
            {
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
                .Include(t => t.Account) // To get the account details associated with each transaction
                .Include(t => t.Category) // To get the category details associated with each transaction
                .Where(t => t.Date >= startDate && t.Date < endDate)
                .Where(t => t.Type == Domain.Enums.TransactionType.Income)
                .ToListAsync();
        }
    }
}
