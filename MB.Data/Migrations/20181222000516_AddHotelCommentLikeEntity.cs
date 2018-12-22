using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MB.Data.Migrations
{
    public partial class AddHotelCommentLikeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "HotelComments");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "HotelComments");

            migrationBuilder.CreateTable(
                name: "HotelCommentLikes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HotelCommentId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelCommentLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelCommentLikes_HotelComments_HotelCommentId",
                        column: x => x.HotelCommentId,
                        principalTable: "HotelComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelCommentLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelCommentLikes_HotelCommentId",
                table: "HotelCommentLikes",
                column: "HotelCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelCommentLikes_UserId",
                table: "HotelCommentLikes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelCommentLikes");

            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "HotelComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "HotelComments",
                nullable: false,
                defaultValue: 0);
        }
    }
}
