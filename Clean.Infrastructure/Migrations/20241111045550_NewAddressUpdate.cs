using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clean.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewAddressUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Address_IsDeleted",
                table: "Employees",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_PostalCode",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Street",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
