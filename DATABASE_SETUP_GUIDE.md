# Personal Finance Tracker - Database Setup Guide

## Overview
Your Personal Finance Tracker project uses Entity Framework Core with SQL Server (LocalDB) as the database. This guide ensures your database is properly configured to fetch and manage user financial data.

---

## Current Database Configuration

### Connection String
- **Location**: `appsettings.json`
- **Server**: `(localdb)\mssqllocaldb`
- **Database Name**: `PersonalFinanceTrackerDb`
- **Authentication**: Windows Authentication (Trusted Connection)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PersonalFinanceTrackerDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

---

## Database Schema

### 1. **AspNetUsers** (Identity Table - Auto-created)
Stores user authentication information.

```
Column Name              | Type           | Description
================================================
Id                       | varchar(128)   | Primary Key
UserName                 | varchar(256)   | Username for login
NormalizedUserName       | varchar(256)   | Uppercase username (for queries)
Email                    | varchar(256)   | User email
NormalizedEmail          | varchar(256)   | Uppercase email
EmailConfirmed           | bit            | Email verification status
PasswordHash             | nvarchar(max)  | Hashed password
SecurityStamp            | nvarchar(max)  | Security token
ConcurrencyStamp         | nvarchar(max)  | Concurrency control
PhoneNumber              | varchar(max)   | User phone
PhoneNumberConfirmed     | bit            | Phone verification status
TwoFactorEnabled         | bit            | 2FA status
LockoutEnd               | datetimeoffset | Account lockout expiration
LockoutEnabled           | bit            | Lockout feature enabled
AccessFailedCount        | int            | Failed login attempts
```

### 2. **Categories** (User-defined transaction categories)
Stores expense and income categories for each user.

```
Column Name       | Type           | Description
================================================
Id                | int            | Primary Key (Auto-increment)
Name              | varchar(max)   | Category name (e.g., "Groceries", "Salary")
TransactionType   | int            | 0 = Income, 1 = Expense
UserId            | varchar(128)   | Foreign Key to AspNetUsers.Id
```

### 3. **Transactions** (User financial transactions)
Stores all income and expense transactions for users.

```
Column Name       | Type           | Description
================================================
Id                | int            | Primary Key (Auto-increment)
Amount            | decimal(18,2)  | Transaction amount
Description       | varchar(max)   | Transaction details
TransactionDate   | datetime2      | Date of transaction
TransactionType   | int            | 0 = Income, 1 = Expense
CategoryId        | int            | Foreign Key to Categories.Id
UserId            | varchar(128)   | Foreign Key to AspNetUsers.Id
CreatedDate       | datetime2      | Record creation timestamp
```

---

## Database Setup Instructions

### Step 1: Verify LocalDB Installation
Ensure SQL Server LocalDB is installed on your system. If not:
```bash
# Download and install SQL Server LocalDB from Microsoft
https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb
```

### Step 2: Apply Migrations
Open **Package Manager Console** in Visual Studio and run:

```powershell
# Update database with existing migrations
Update-Database

# Verify migration status
Get-Migration
```

**Expected Output**: Shows migrations applied (InitialCreate, transactionandcategories, categorymodelupdate)

### Step 3: Verify Database Creation
The database `PersonalFinanceTrackerDb` will be created automatically with all required tables:
- AspNetUsers (Identity)
- AspNetRoles (Identity)
- AspNetUserRoles (Identity)
- Categories
- Transactions
- AspNetUserClaims, AspNetRoleClaims, AspNetUserTokens, etc.

### Step 4: Test Database Connection
You can verify the connection by viewing the database in **SQL Server Object Explorer**:

1. In Visual Studio, open **View ? SQL Server Object Explorer**
2. Expand **(localdb)\mssqllocaldb** ? **Databases**
3. Look for **PersonalFinanceTrackerDb**
4. Expand it to see tables: AspNetUsers, Categories, Transactions

---

## Creating a New Migration (If Adding Features)

### When to Create a Migration
When you modify models (add/remove/change properties), create a new migration:

```powershell
# Create migration with description
Add-Migration YourMigrationDescription

# Apply migration to database
Update-Database
```

### Example: Adding a Field to Transaction
1. Modify `Models/TransactionModel.cs`:
```csharp
public class TransactionModel
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime TransactionDate { get; set; }
    public int TransactionType { get; set; }
    public int CategoryId { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Notes { get; set; } // New field
}
```

2. Create migration:
```powershell
Add-Migration AddNotesFieldToTransaction
```

3. Update database:
```powershell
Update-Database
```

---

## Data Access Patterns

### Repositories Used in Project
Your project uses the Repository Pattern for data access:

- **TransactionRepository** (`Repository/TransactionRepository.cs`)
  - Methods: GetUserTransactions(), AddTransaction(), UpdateTransaction(), DeleteTransaction()

- **CategoryRepository** (`Repository/CategoryRepository.cs`)
  - Methods: GetUserCategories(), AddCategory(), UpdateCategory(), DeleteCategory()

### Services Used in Project
Services implement business logic:

- **TransactionService** (`Service/TransactionService.cs`)
  - Handles transaction business logic and validation

- **CategoryService** (`Service/CategoryService.cs`)
  - Handles category business logic

---

## Fetching User Data - Common Patterns

### 1. Get All Transactions for Logged-in User
```csharp
// In Controller
using System.Security.Claims;

public async Task<IActionResult> Index()
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var transactions = await _transactionService.GetUserTransactionsAsync(userId);
    return View(transactions);
}
```

### 2. Get Transactions by Category for User
```csharp
var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
var expenseTransactions = await _transactionService.GetTransactionsByTypeAsync(userId, 1); // 1 = Expense
```

### 3. Get Income and Expense Summary
```csharp
var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
var totalIncome = await _transactionService.GetTotalIncomeAsync(userId);
var totalExpense = await _transactionService.GetTotalExpenseAsync(userId);
```

### 4. Get Categories for User
```csharp
var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
var categories = await _categoryService.GetUserCategoriesAsync(userId);
```

---

## Database Best Practices Implemented

? **User Data Isolation**: Every transaction and category is tied to `UserId`  
? **Audit Trail**: `CreatedDate` field tracks record creation  
? **Soft Deletes Potential**: Can be added to Categories/Transactions for data retention  
? **Proper Indexing**: UserId fields should be indexed for performance  
? **Entity Relationships**: Foreign keys enforce referential integrity  
? **Identity Management**: Built-in ASP.NET Identity for authentication  

---

## Troubleshooting

### Issue: Database Not Found
**Solution**: Run `Update-Database` in Package Manager Console

### Issue: Migration Conflict
**Solution**: 
```powershell
Remove-Migration  # Removes last unapplied migration
Add-Migration YourMigrationName  # Create new migration
Update-Database
```

### Issue: LocalDB Not Running
**Solution**: 
```bash
# Start LocalDB instance
sqllocaldb start mssqllocaldb

# Check status
sqllocaldb info mssqllocaldb
```

### Issue: Connection String Error
**Solution**: Verify `appsettings.json` has correct connection string and LocalDB is installed

---

## Additional Recommended Enhancements

### 1. Add Data Indexing
Update `Data/ApplicationDbContext.cs` to add indexes for performance:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Index for faster user queries
    modelBuilder.Entity<TransactionModel>()
        .HasIndex(t => t.UserId);

    modelBuilder.Entity<CategoryModel>()
        .HasIndex(c => c.UserId);
}
```

### 2. Add Soft Delete Support
Add `IsDeleted` field to models:
```csharp
public class TransactionModel
{
    // ... existing properties
    public bool IsDeleted { get; set; } = false;
}
```

### 3. Add Budget Tracking
Create new `BudgetModel` for monthly budgets:
```csharp
public class BudgetModel
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public decimal MonthlyLimit { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public string UserId { get; set; }
}
```

---

## Quick Reference Commands

```powershell
# View applied migrations
Get-Migration

# Add new migration
Add-Migration MigrationName

# Apply all pending migrations
Update-Database

# Revert to specific migration
Update-Database -Migration MigrationName

# Remove last unapplied migration
Remove-Migration

# View database in Object Explorer
# View ? SQL Server Object Explorer ? (localdb)\mssqllocaldb
```

---

## Summary

Your Personal Finance Tracker database is ready to fetch user financial data:
- ? SQL Server LocalDB configured
- ? Entity Framework Core properly set up
- ? Migrations applied
- ? User authentication integrated
- ? Category and Transaction tables created
- ? Repository and Service pattern implemented

Users can now register, log in, and manage their transactions and categories with complete data isolation!
