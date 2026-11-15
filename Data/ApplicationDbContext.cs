using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<TransactionModel> Transactions { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ApplicationUser
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.ContactDetails)
                    .HasMaxLength(100);

                entity.Property(e => e.CurrentLocation)
                    .HasMaxLength(200);

                entity.Property(e => e.Message)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedDate)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                // Add index for CreatedDate queries
                entity.HasIndex(e => e.CreatedDate)
                    .HasName("IX_ApplicationUser_CreatedDate");
            });

            // Configure CategoryModel
            modelBuilder.Entity<CategoryModel>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.Property(e => e.TransactionType)
                    .IsRequired();

                // Add index for UserId queries
                entity.HasIndex(e => e.UserId)
                    .HasName("IX_Categories_UserId");
            });

            // Configure TransactionModel
            modelBuilder.Entity<TransactionModel>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Amount)
                    .HasPrecision(18, 2)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.TransactionDate)
                    .IsRequired();

                entity.Property(e => e.TransactionType)
                    .IsRequired();

                entity.Property(e => e.CategoryId)
                    .IsRequired();

                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.Property(e => e.CreatedDate)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                // Add indexes for common queries
                entity.HasIndex(e => e.UserId)
                    .HasName("IX_Transactions_UserId");

                entity.HasIndex(e => new { e.UserId, e.TransactionDate })
                    .HasName("IX_Transactions_UserId_TransactionDate");

                entity.HasIndex(e => e.CategoryId)
                    .HasName("IX_Transactions_CategoryId");
            });
        }
    }
}
