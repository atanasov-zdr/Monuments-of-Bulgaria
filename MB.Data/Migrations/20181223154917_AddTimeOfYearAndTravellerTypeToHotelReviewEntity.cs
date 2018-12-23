using Microsoft.EntityFrameworkCore.Migrations;

namespace MB.Data.Migrations
{
    public partial class AddTimeOfYearAndTravellerTypeToHotelReviewEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeOfYear",
                table: "HotelReviews",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TravellerType",
                table: "HotelReviews",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOfYear",
                table: "HotelReviews");

            migrationBuilder.DropColumn(
                name: "TravellerType",
                table: "HotelReviews");
        }
    }
}
