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
            return await _context.Transactions.ToListAsync();
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
            return await query.ToListAsync();
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
    }
}
