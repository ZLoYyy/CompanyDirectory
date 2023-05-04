using Microsoft.EntityFrameworkCore.Migrations;

namespace CompanyDirectory.Server.Migrations
{
    public partial class UpdateDivisionAndEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DirectorID",
                table: "Divisions");

            migrationBuilder.AddColumn<bool>(
                name: "IsDirector",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDirector",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "DirectorID",
                table: "Divisions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
