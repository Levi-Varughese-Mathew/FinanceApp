using FinanceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Data
{
    public class FinanceDbContext : DbContext
    {

        // Setting up the db context for the EF core to generate tables
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options) { }

        // DbSet properties for each entity in the domain model
        // These properties represent the tables in the database.
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Budget> Budgets => Set<Budget>();
        public DbSet<Debt> Debts => Set<Debt>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();

    }
}
