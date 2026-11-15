using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationUserWithProfileFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add new columns to AspNetUsers table
            migrationBuilder.AddColumn<string>(
                name: "ContactDetails",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentLocation",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            // Create index on CreatedDate
            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_CreatedDate",
                table: "AspNetUsers",
                column: "CreatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop index
            migrationBuilder.DropIndex(
                name: "IX_ApplicationUser_CreatedDate",
                table: "AspNetUsers");

            // Remove columns
            migrationBuilder.DropColumn(
                name: "ContactDetails",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CurrentLocation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AspNetUsers");
        }
    }
}
