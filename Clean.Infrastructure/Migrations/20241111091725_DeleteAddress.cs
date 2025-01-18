using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clean.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_IsDeleted",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Address_IsDeleted",
                table: "Employees",
                type: "bit",
                nullable: true);
        }
    }
}
