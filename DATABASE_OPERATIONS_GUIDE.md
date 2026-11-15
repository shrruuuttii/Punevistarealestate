# Database Operations - Quick Reference Guide

## Database Structure Overview

Your Personal Finance Tracker uses Entity Framework Core with SQL Server LocalDB. Here's the complete data model:

```
???????????????????????????????????????????????????????
?              AspNetUsers (Identity)                 ?
?  Stores user authentication & profile information  ?
???????????????????????????????????????????????????????
? Id (PK) | UserName | Email | Password Hash | ...   ?
???????????????????????????????????????????????????????
                        ?
                        ? UserId FK
                        ?
          ??????????????????????????????
          ?                            ?
????????????????????????????  ??????????????????????????
?   Categories             ?  ?   Transactions         ?
????????????????????????????  ??????????????????????????
? Id (PK)                  ?  ? Id (PK)                ?
? Name                     ?  ? Amount                 ?
? TransactionType (0/1)    ?  ? Description            ?
? UserId (FK)              ?  ? TransactionDate        ?
?                          ?  ? TransactionType (0/1)  ?
? IX_Categories_UserId ?   ?  ? CategoryId (FK) ?????  ?
????????????????????????????  ? UserId (FK)         ?  ?
                              ? CreatedDate         ?  ?
                              ?                    ?  ?
                              ? Indexes:           ?  ?
                              ? ? IX_Transactions   ?  ?
                              ?   _UserId           ?  ?
                              ? ? IX_Transactions   ?  ?
                              ?   _UserId_Date      ?  ?
                              ? ? IX_Transactions   ?  ?
                              ?   _CategoryId ???????  ?
                              ??????????????????????????

Legend:
PK = Primary Key
FK = Foreign Key
?  = Indexed for performance
0  = Income (for TransactionType)
1  = Expense (for TransactionType)
```

---

## Common Database Operations

### 1. Fetching User Transactions

**In Controller:**
```csharp
using System.Security.Claims;

public class TransactionController : Controller
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<IActionResult> Index()
    {
        // Get currently logged-in user ID
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        // Fetch all transactions for this user
        var transactions = await _transactionService.GetUserTransactionsAsync(userId);
        return View(transactions);
    }
}
```

### 2. Get Transactions with Category Details

**Repository Method (Already Implemented):**
```csharp
// In TransactionRepository
public async Task<List<TransactionWithCategoryDto>> GetTransactionsWithCategoryAsync(string userId)
{
    return await _dbContext.Transactions
        .Where(t => t.UserId == userId)
        .Join(_dbContext.Categories,
            t => t.CategoryId,
            c => c.Id,
            (t, c) => new TransactionWithCategoryDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description,
                TransactionDate = t.TransactionDate,
                TransactionType = t.TransactionType,
                CategoryName = c.Name
            })
        .ToListAsync();
}
```

**Usage in Controller:**
```csharp
var transactionsWithCategory = await _transactionService
    .GetTransactionsWithCategoryAsync(userId);
```

### 3. Creating a New Transaction

**In Controller:**
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(TransactionModel transaction)
{
    if (ModelState.IsValid)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        transaction.UserId = userId;
        transaction.CreatedDate = DateTime.UtcNow;

        var success = await _transactionService.AddTransactionAsync(transaction);

        if (success)
            return RedirectToAction("Index");
        else
            ModelState.AddModelError("", "Failed to add transaction");
    }
    return View(transaction);
}
```

### 4. Getting Income vs Expense Summary

**Using DatabaseInitializer:**
```csharp
[HttpGet]
public async Task<IActionResult> Dashboard()
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    var (totalIncome, totalExpense, txnCount, categoryCount) = 
        await DatabaseInitializer.GetUserSummaryAsync(_dbContext, userId);

    ViewBag.TotalIncome = totalIncome;
    ViewBag.TotalExpense = totalExpense;
    ViewBag.NetBalance = totalIncome - totalExpense;
    ViewBag.TransactionCount = txnCount;

    return View();
}
```

### 5. Get Monthly Breakdown

**Using DatabaseInitializer:**
```csharp
[HttpGet]
public async Task<IActionResult> MonthlyReport(int month, int year)
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    // Get all transactions for the month
    var transactions = await DatabaseInitializer
        .GetTransactionsByMonthAsync(_dbContext, userId, month, year);

    // Get spending by category
    var spendingByCategory = await DatabaseInitializer
        .GetMonthlySpendingByCategoryAsync(_dbContext, userId, month, year);

    ViewBag.Month = month;
    ViewBag.Year = year;
    ViewBag.SpendingByCategory = spendingByCategory;

    return View(transactions);
}
```

### 6. Fetching User Categories

**In Controller:**
```csharp
public async Task<IActionResult> GetCategories()
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    // Get only expense categories
    var expenseCategories = await _categoryService
        .GetCategoriesByTypeAsync(userId, 1); // 1 = Expense

    // Get only income categories
    var incomeCategories = await _categoryService
        .GetCategoriesByTypeAsync(userId, 0); // 0 = Income

    return Json(new { 
        expenses = expenseCategories, 
        incomes = incomeCategories 
    });
}
```

### 7. Adding a Category

**In Controller:**
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AddCategory(CategoryModel category)
{
    if (ModelState.IsValid)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        category.UserId = userId;

        var success = await _categoryService.AddCategoryAsync(category);

        if (success)
            return RedirectToAction("Index");
        else
            ModelState.AddModelError("", "Failed to add category");
    }
    return View(category);
}
```

### 8. Searching/Filtering Transactions

**By Date Range:**
```csharp
public async Task<List<TransactionModel>> GetTransactionsByDateRange(
    string userId, DateTime startDate, DateTime endDate)
{
    return await _dbContext.Transactions
        .Where(t => t.UserId == userId 
            && t.TransactionDate >= startDate 
            && t.TransactionDate <= endDate)
        .OrderByDescending(t => t.TransactionDate)
        .ToListAsync();
}
```

**By Amount Range:**
```csharp
public async Task<List<TransactionModel>> GetTransactionsByAmountRange(
    string userId, decimal minAmount, decimal maxAmount)
{
    return await _dbContext.Transactions
        .Where(t => t.UserId == userId 
            && t.Amount >= minAmount 
            && t.Amount <= maxAmount)
        .OrderByDescending(t => t.Amount)
        .ToListAsync();
}
```

**By Transaction Type:**
```csharp
// 0 = Income, 1 = Expense
public async Task<List<TransactionModel>> GetExpenses(string userId)
{
    return await _dbContext.Transactions
        .Where(t => t.UserId == userId && t.TransactionType == 1)
        .OrderByDescending(t => t.TransactionDate)
        .ToListAsync();
}

public async Task<List<TransactionModel>> GetIncome(string userId)
{
    return await _dbContext.Transactions
        .Where(t => t.UserId == userId && t.TransactionType == 0)
        .OrderByDescending(t => t.TransactionDate)
        .ToListAsync();
}
```

---

## Razor View Examples

### Display Transaction List

```html
@model List<TransactionModel>

<div class="table-responsive">
    <table class="table table-striped">
        <thead>
            <tr>
                <th>Date</th>
                <th>Description</th>
                <th>Category</th>
                <th>Amount</th>
                <th>Type</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var transaction in Model)
            {
                <tr>
                    <td>@transaction.TransactionDate.ToString("MMM dd, yyyy")</td>
                    <td>@transaction.Description</td>
                    <td>
                        @{
                            var category = await _categoryService.GetCategoryByIdAsync(transaction.CategoryId);
                            <span>@category?.Name</span>
                        }
                    </td>
                    <td>? @transaction.Amount.ToString("N2")</td>
                    <td>
                        @if (transaction.TransactionType == 0)
                        {
                            <span class="badge bg-success">Income</span>
                        }
                        else
                        {
                            <span class="badge bg-danger">Expense</span>
                        }
                    </td>
                    <td>
                        <a href="@Url.Action("Edit", new { id = transaction.Id })" class="btn btn-sm btn-warning">Edit</a>
                        <a href="@Url.Action("Delete", new { id = transaction.Id })" class="btn btn-sm btn-danger">Delete</a>
                    </td>
                </tr>
            }
        </tbody>
    </table>
</div>
```

### Display Summary Card

```html
@{
    var (totalIncome, totalExpense, _, _) = 
        await DatabaseInitializer.GetUserSummaryAsync(DbContext, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
}

<div class="row">
    <div class="col-md-6">
        <div class="card text-white bg-success">
            <div class="card-body">
                <h5 class="card-title">Total Income</h5>
                <p class="card-text fs-4">? @totalIncome.ToString("N2")</p>
            </div>
        </div>
    </div>
    <div class="col-md-6">
        <div class="card text-white bg-danger">
            <div class="card-body">
                <h5 class="card-title">Total Expense</h5>
                <p class="card-text fs-4">? @totalExpense.ToString("N2")</p>
            </div>
        </div>
    </div>
</div>
```

---

## Database Performance Tips

### 1. **Use Indexes Efficiently**
The database is configured with indexes on:
- `UserId` (Categories & Transactions) - for fast user data filtering
- `UserId + TransactionDate` (Transactions) - for date range queries
- `CategoryId` (Transactions) - for category lookups

These are automatically created by migrations.

### 2. **Pagination for Large Datasets**
```csharp
public async Task<List<TransactionModel>> GetUserTransactionsPagedAsync(
    string userId, int pageNumber, int pageSize = 10)
{
    return await _dbContext.Transactions
        .Where(t => t.UserId == userId)
        .OrderByDescending(t => t.TransactionDate)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
}
```

### 3. **Use Include() for Related Data**
```csharp
var transactions = await _dbContext.Transactions
    .Where(t => t.UserId == userId)
    .Include(t => t.Category) // Load category data
    .ToListAsync();
```

### 4. **Async Operations**
Always use async methods (`ToListAsync()`, `FirstOrDefaultAsync()`, `SaveChangesAsync()`) to avoid blocking threads.

---

## Troubleshooting

### Database Connection Failed
```powershell
# Verify LocalDB is running
sqllocaldb start mssqllocaldb
sqllocaldb info mssqllocaldb
```

### Migration Errors
```powershell
# Remove last migration and try again
Remove-Migration

# View all migrations
Get-Migration

# Revert to specific migration
Update-Database -Migration PreviousMigrationName
```

### Data Not Showing
1. Check `UserId` is being set correctly
2. Verify user is logged in: `User.Identity.IsAuthenticated`
3. Check connection string in `appsettings.json`
4. Run migrations: `Update-Database`

---

## Summary

? Database properly configured with Entity Framework Core  
? User data isolation via UserId foreign keys  
? Indexes created for performance optimization  
? Migration system ready for schema changes  
? DatabaseInitializer helper for common operations  
? Repository pattern for clean data access  
? Ready for production deployment
