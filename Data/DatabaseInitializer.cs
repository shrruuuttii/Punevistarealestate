using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Data
{
    /// <summary>
    /// Helper class to initialize and seed the database with default categories for new users
    /// </summary>
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Initializes the database by applying all pending migrations
        /// Call this in Program.cs during application startup
        /// </summary>
        public static async Task InitializeDatabaseAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Apply pending migrations
                try
                {
                    await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    // Log migration errors but don't fail application startup
                    Console.WriteLine($"Database migration error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Seeds default expense and income categories for a user
        /// Call this when a new user registers
        /// </summary>
        public static async Task SeedDefaultCategoriesForUserAsync(
            ApplicationDbContext context, 
            string userId)
        {
            // Check if user already has categories
            var userHasCategories = await context.Categories
                .AnyAsync(c => c.UserId == userId);

            if (userHasCategories)
                return;

            // Default expense categories
            var expenseCategories = new List<CategoryModel>
            {
                new CategoryModel { Name = "Groceries", TransactionType = 1, UserId = userId },
                new CategoryModel { Name = "Utilities", TransactionType = 1, UserId = userId },
                new CategoryModel { Name = "Entertainment", TransactionType = 1, UserId = userId },
                new CategoryModel { Name = "Healthcare", TransactionType = 1, UserId = userId },
                new CategoryModel { Name = "Transportation", TransactionType = 1, UserId = userId },
                new CategoryModel { Name = "Dining Out", TransactionType = 1, UserId = userId },
                new CategoryModel { Name = "Shopping", TransactionType = 1, UserId = userId },
                new CategoryModel { Name = "Subscriptions", TransactionType = 1, UserId = userId },
                new CategoryModel { Name = "Insurance", TransactionType = 1, UserId = userId },
                new CategoryModel { Name = "Rent/Mortgage", TransactionType = 1, UserId = userId }
            };

            // Default income categories
            var incomeCategories = new List<CategoryModel>
            {
                new CategoryModel { Name = "Salary", TransactionType = 0, UserId = userId },
                new CategoryModel { Name = "Freelance", TransactionType = 0, UserId = userId },
                new CategoryModel { Name = "Investment Returns", TransactionType = 0, UserId = userId },
                new CategoryModel { Name = "Bonus", TransactionType = 0, UserId = userId },
                new CategoryModel { Name = "Other Income", TransactionType = 0, UserId = userId }
            };

            var allCategories = expenseCategories.Concat(incomeCategories).ToList();

            context.Categories.AddRange(allCategories);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets summary statistics for a user's financial data
        /// </summary>
        public static async Task<(decimal TotalIncome, decimal TotalExpense, int TransactionCount, int CategoryCount)> 
            GetUserSummaryAsync(ApplicationDbContext context, string userId)
        {
            var transactions = await context.Transactions
                .Where(t => t.UserId == userId)
                .ToListAsync();

            var totalIncome = transactions
                .Where(t => t.TransactionType == 0) // Income
                .Sum(t => t.Amount);

            var totalExpense = transactions
                .Where(t => t.TransactionType == 1) // Expense
                .Sum(t => t.Amount);

            var transactionCount = transactions.Count;

            var categoryCount = await context.Categories
                .Where(c => c.UserId == userId)
                .CountAsync();

            return (totalIncome, totalExpense, transactionCount, categoryCount);
        }

        /// <summary>
        /// Validates database connectivity
        /// Returns true if database is accessible, false otherwise
        /// </summary>
        public static async Task<bool> IsDataBaseConnectedAsync(ApplicationDbContext context)
        {
            try
            {
                return await context.Database.CanConnectAsync();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets recent transactions for a user
        /// </summary>
        public static async Task<List<TransactionModel>> GetRecentTransactionsAsync(
            ApplicationDbContext context, 
            string userId, 
            int limit = 10)
        {
            return await context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.TransactionDate)
                .Take(limit)
                .ToListAsync();
        }

        /// <summary>
        /// Gets transactions for a specific month
        /// </summary>
        public static async Task<List<TransactionModel>> GetTransactionsByMonthAsync(
            ApplicationDbContext context, 
            string userId, 
            int month, 
            int year)
        {
            return await context.Transactions
                .Where(t => t.UserId == userId 
                    && t.TransactionDate.Month == month 
                    && t.TransactionDate.Year == year)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets spending breakdown by category for a specific month
        /// </summary>
        public static async Task<Dictionary<string, decimal>> GetMonthlySpendingByCategoryAsync(
            ApplicationDbContext context, 
            string userId, 
            int month, 
            int year)
        {
            var transactions = await context.Transactions
                .Where(t => t.UserId == userId 
                    && t.TransactionType == 1 // Expenses only
                    && t.TransactionDate.Month == month 
                    && t.TransactionDate.Year == year)
                .Include(t => context.Categories.Where(c => c.Id == t.CategoryId))
                .ToListAsync();

            var categoryNames = await context.Categories
                .Where(c => c.UserId == userId)
                .ToDictionaryAsync(c => c.Id, c => c.Name);

            var breakdown = transactions
                .GroupBy(t => t.CategoryId)
                .ToDictionary(
                    g => categoryNames.ContainsKey(g.Key) ? categoryNames[g.Key] : "Unknown",
                    g => g.Sum(t => t.Amount));

            return breakdown;
        }
    }
}
