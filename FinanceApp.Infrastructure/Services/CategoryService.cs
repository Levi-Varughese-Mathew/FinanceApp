using System;
using System.Collections.Generic;
using System.Text;
using FinanceApp.Domain.Models;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace FinanceApp.Infrastructure.Services
{
    public class CategoryService
    {
        // Getting the db context from the DI container and using it to perform CRUD operations on the Category entity.
        private readonly FinanceDbContext _context;

        // Declaring the constructor for the CategoryService class, which takes a FinanceDbContext as a parameter and assigns it to the _context field.
        public CategoryService(FinanceDbContext context)
        {
            _context = context;
        }

        // Getting all the categories (to use in the dropdown list)
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        // Getting only expense categories (to use in the dropdown list)
        public async Task<List<Category>> GetExpenseCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.TransactionType == Domain.Enums.TransactionType.Expense)
                .ToListAsync();
        }

        // Getting only income categories (to use in the dropdown list)
        public async Task<List<Category>> GetIncomeCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.TransactionType == Domain.Enums.TransactionType.Income)
                .ToListAsync();
        }

        // Add a new category
        public async Task AddAccountAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
    }
}
