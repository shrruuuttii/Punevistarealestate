# Quick Start Guide - Personal Finance Tracker Database

## ? 5-Minute Setup

### Prerequisites
- Visual Studio 2022 or later
- .NET 9 SDK installed
- SQL Server LocalDB (installed with Visual Studio)

### Steps

1. **Open Solution**
   ```
   Open PersonalFinanceTracker.sln in Visual Studio
   ```

2. **Build Project**
   ```
   Press Ctrl+Shift+B or Build ? Build Solution
   ```

3. **Run Application**
   ```
   Press Ctrl+F5 or Debug ? Start Without Debugging
   ```

   ? Database will be created automatically
   ? Migrations will be applied
   ? Application is ready to use

4. **Create Account**
   - Click "Register"
   - Enter email and password
   - Verify email
   - Log in

5. **Add Transaction**
   - Go to Transactions section
   - Click "Add Transaction"
   - Select category, enter amount, date
   - Click Save

6. **View Dashboard**
   - Income, Expense, and Balance displayed
   - Recent transactions listed
   - Monthly summary available

---

## ?? Database Tables Overview

| Table | Purpose | Key Fields |
|-------|---------|-----------|
| AspNetUsers | User authentication | Id, UserName, Email, PasswordHash |
| Categories | Transaction categories | Id, Name, TransactionType, UserId |
| Transactions | Financial records | Id, Amount, TransactionDate, UserId, CategoryId |

---

## ?? Common Operations

### Get Current User ID (In Controller)
```csharp
using System.Security.Claims;

var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
```

### Fetch User's Transactions
```csharp
var transactions = await _transactionService.GetUserTransactionsAsync(userId);
```

### Get Income + Expense Summary
```csharp
var (income, expense, count, cats) = 
    await DatabaseInitializer.GetUserSummaryAsync(_dbContext, userId);

var balance = income - expense;
```

### Add New Category
```csharp
var category = new CategoryModel
{
    Name = "Shopping",
    TransactionType = 1, // 1 = Expense
    UserId = userId
};
await _categoryService.AddCategoryAsync(category);
```

### Get Monthly Spending Breakdown
```csharp
var spendingByCategory = await DatabaseInitializer
    .GetMonthlySpendingByCategoryAsync(_dbContext, userId, month, year);

// Result: Dictionary<string, decimal>
// Example: { "Groceries": 5000, "Utilities": 2000, ... }
```

---

## ?? Transaction Types

```
TransactionType = 0  ?  INCOME
  - Salary
  - Freelance
  - Investment Returns
  - Bonus
  - Other Income

TransactionType = 1  ?  EXPENSE
  - Groceries
  - Utilities
  - Entertainment
  - Healthcare
  - Transportation
  - Dining Out
  - Shopping
  - Subscriptions
  - Insurance
  - Rent/Mortgage
```

---

## ??? Database Connection String

Located in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PersonalFinanceTrackerDb;Trusted_Connection=true;MultipleActiveResultSets=true"
}
```

**To change database:**
1. Edit the connection string in `appsettings.json`
2. Delete the old database (if using LocalDB)
3. Run `Update-Database` in Package Manager Console
4. Restart application

---

## ?? Troubleshooting

### Application won't start
```powershell
# Try resetting LocalDB
sqllocaldb stop mssqllocaldb
sqllocaldb start mssqllocaldb
```

### Database not found
```powershell
# Open Package Manager Console and run:
Update-Database
```

### Migrations failed
```powershell
# Revert last migration:
Remove-Migration

# Check migration status:
Get-Migration

# Apply all pending migrations:
Update-Database
```

### User can't see transactions
1. Verify user is logged in: `User.Identity.IsAuthenticated`
2. Ensure `UserId` is being set when creating transactions
3. Check `CreatedDate` is populated

---

## ?? Project Structure

```
PersonalFinanceTracker/
??? Models/
?   ??? TransactionModel.cs
?   ??? CategoryModel.cs
?   ??? ViewModels/
??? Repository/
?   ??? TransactionRepository.cs
?   ??? CategoryRepository.cs
??? Service/
?   ??? TransactionService.cs
?   ??? CategoryService.cs
??? Controllers/
?   ??? TransactionController.cs
?   ??? CategoryController.cs
?   ??? DashboardController.cs
??? Data/
?   ??? ApplicationDbContext.cs
?   ??? DatabaseInitializer.cs
?   ??? Migrations/
??? Views/
?   ??? Transaction/
?   ??? Category/
?   ??? Dashboard/
??? Program.cs
```

---

## ?? Security Features

? **User Isolation**: Each user's transactions are isolated using `UserId`
? **Authentication**: AspNetCore Identity for secure login
? **Authorization**: Only logged-in users can access features
? **CSRF Protection**: Built-in token validation on forms
? **Password Hashing**: Passwords never stored in plain text

---

## ?? Performance

? **Database Indexes**:
- `IX_Categories_UserId` - Fast category lookups
- `IX_Transactions_UserId` - Fast transaction filtering by user
- `IX_Transactions_UserId_TransactionDate` - Optimized for date range queries
- `IX_Transactions_CategoryId` - Fast category joins

? **Async/Await**: All database operations are async to prevent thread blocking
? **Lazy Loading**: Data loaded on-demand
? **Query Optimization**: EF Core generates efficient SQL queries

---

## ?? Data Backup

To backup your data:

**Using SQL Server Management Studio:**
1. Right-click database `PersonalFinanceTrackerDb`
2. Tasks ? Back Up
3. Select backup location and name
4. Click OK

**Using PowerShell:**
```powershell
# Export database to SQL file
sqlcmd -S "(localdb)\mssqllocaldb" -Q "BACKUP DATABASE PersonalFinanceTrackerDb TO DISK='C:\Backup\PFT.bak'"
```

---

## ?? Deployment Checklist

Before deploying to production:

- [ ] Change connection string to production database
- [ ] Set `RequireConfirmedAccount = false` (or use email confirmation service)
- [ ] Enable HTTPS
- [ ] Set up proper logging
- [ ] Configure application settings in `appsettings.Production.json`
- [ ] Test all database operations
- [ ] Back up production database
- [ ] Set up automated daily backups
- [ ] Configure database indexes for production scale

---

## ?? Need Help?

1. **Check Logs**: Look at Visual Studio Output window for error messages
2. **Database Explorer**: View ? SQL Server Object Explorer to inspect database
3. **Documentation**: Read `DATABASE_SETUP_GUIDE.md` for detailed info
4. **Examples**: See `DATABASE_OPERATIONS_GUIDE.md` for code examples

---

## ? What's Configured

| Component | Status |
|-----------|--------|
| SQL Server LocalDB | ? Configured |
| Entity Framework Core | ? Set up |
| Database Migrations | ? Ready |
| User Authentication | ? Integrated |
| Repository Pattern | ? Implemented |
| Service Layer | ? Implemented |
| Indexes & Performance | ? Optimized |
| Database Initialization | ? Automatic |
| Error Handling | ? Configured |

---

**You're all set! Start building! ??**
