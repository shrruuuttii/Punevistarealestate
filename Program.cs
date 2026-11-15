using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Interface;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Service;
using PersonalFinanceTracker.Repository;

namespace PersonalFinanceTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           // Add services to the container.
           var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
           builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString));
           builder.Services.AddDatabaseDeveloperPageExceptionFilter();

           builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddEntityFrameworkStores<ApplicationDbContext>();
           builder.Services.AddControllersWithViews();

           // Dependency Injection Registration
           builder.Services.AddScoped<ITransactionService, TransactionService>();
           builder.Services.AddScoped<ICategoryService, CategoryService>();
           builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
           builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

           var app = builder.Build();

           // Initialize database - apply migrations automatically on startup
           DatabaseInitializer.InitializeDatabaseAsync(app).GetAwaiter().GetResult();

           // Configure the HTTP request pipeline.
           if (app.Environment.IsDevelopment())
           {
               app.UseMigrationsEndPoint();
           }
           else
           {
               app.UseExceptionHandler("/Home/Error");
               app.UseHsts();
           }

           app.UseHttpsRedirection();
           app.UseRouting();

           app.UseAuthorization();

           app.MapStaticAssets();
           app.MapControllerRoute(
               name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}")
               .WithStaticAssets();
           app.MapRazorPages()
              .WithStaticAssets();

           app.Run();
        }
    }
}
