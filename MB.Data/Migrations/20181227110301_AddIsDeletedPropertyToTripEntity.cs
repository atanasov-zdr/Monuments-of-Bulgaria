using Microsoft.EntityFrameworkCore.Migrations;

namespace MB.Data.Migrations
{
    public partial class AddIsDeletedPropertyToTripEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Trips",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Trips");
        }
    }
}
