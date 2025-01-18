using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clean.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Address_IsDeleted",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Address_PostalCode",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Address_Street",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "Employees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Address_IsDeleted",
                table: "Employees",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_PostalCode",
                table: "Employees",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Street",
                table: "Employees",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
