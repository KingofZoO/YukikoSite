using Microsoft.EntityFrameworkCore.Migrations;

namespace YukikoSite.Migrations
{
    public partial class LinkField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LinkPath",
                table: "Ventilation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkPath",
                table: "SidingComplect",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkPath",
                table: "Siding",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkPath",
                table: "Others",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkPath",
                table: "Gloves",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkPath",
                table: "FibrosComplect",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkPath",
                table: "Fibros16",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkPath",
                table: "Fibros14",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkPath",
                table: "Ventilation");

            migrationBuilder.DropColumn(
                name: "LinkPath",
                table: "SidingComplect");

            migrationBuilder.DropColumn(
                name: "LinkPath",
                table: "Siding");

            migrationBuilder.DropColumn(
                name: "LinkPath",
                table: "Others");

            migrationBuilder.DropColumn(
                name: "LinkPath",
                table: "Gloves");

            migrationBuilder.DropColumn(
                name: "LinkPath",
                table: "FibrosComplect");

            migrationBuilder.DropColumn(
                name: "LinkPath",
                table: "Fibros16");

            migrationBuilder.DropColumn(
                name: "LinkPath",
                table: "Fibros14");
        }
    }
}
