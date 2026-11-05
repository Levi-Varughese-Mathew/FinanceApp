using System;
using System.Collections.Generic;
using System.Text;
using FinanceApp.Domain.Models;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Services
{
    public class AccountService
    {
        // Getting the db context from the DI container and using it to perform CRUD operations on the Account entity.
        private readonly FinanceDbContext _context;

        // Declaring the constructor for the AccountService class, which takes a FinanceDbContext as a parameter and assigns it to the _context field.
        public AccountService(FinanceDbContext context)
        {
            _context = context;
        }

        // Getting all the accounts
        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        // Add a new account
        public async Task AddAccountAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }
    }
}
