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
        public async Task<List<Account>> GetAllCategoriesAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        // Add a new category
        public async Task AddAccountAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
    }
}
