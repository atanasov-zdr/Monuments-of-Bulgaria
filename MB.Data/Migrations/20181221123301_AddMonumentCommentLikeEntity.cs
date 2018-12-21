using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MB.Data.Migrations
{
    public partial class AddMonumentCommentLikeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "MonumentComments");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "MonumentComments");

            migrationBuilder.CreateTable(
                name: "MonumentCommentLike",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MonumentCommentId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonumentCommentLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonumentCommentLike_MonumentComments_MonumentCommentId",
                        column: x => x.MonumentCommentId,
                        principalTable: "MonumentComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonumentCommentLike_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonumentCommentLike_MonumentCommentId",
                table: "MonumentCommentLike",
                column: "MonumentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_MonumentCommentLike_UserId",
                table: "MonumentCommentLike",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonumentCommentLike");

            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "MonumentComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "MonumentComments",
                nullable: false,
                defaultValue: 0);
        }
    }
}
