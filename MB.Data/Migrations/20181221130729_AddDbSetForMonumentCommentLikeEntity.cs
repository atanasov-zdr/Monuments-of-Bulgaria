using Microsoft.EntityFrameworkCore.Migrations;

namespace MB.Data.Migrations
{
    public partial class AddDbSetForMonumentCommentLikeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonumentCommentLike_MonumentComments_MonumentCommentId",
                table: "MonumentCommentLike");

            migrationBuilder.DropForeignKey(
                name: "FK_MonumentCommentLike_AspNetUsers_UserId",
                table: "MonumentCommentLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonumentCommentLike",
                table: "MonumentCommentLike");

            migrationBuilder.RenameTable(
                name: "MonumentCommentLike",
                newName: "MonumentCommentLikes");

            migrationBuilder.RenameIndex(
                name: "IX_MonumentCommentLike_UserId",
                table: "MonumentCommentLikes",
                newName: "IX_MonumentCommentLikes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MonumentCommentLike_MonumentCommentId",
                table: "MonumentCommentLikes",
                newName: "IX_MonumentCommentLikes_MonumentCommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonumentCommentLikes",
                table: "MonumentCommentLikes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MonumentCommentLikes_MonumentComments_MonumentCommentId",
                table: "MonumentCommentLikes",
                column: "MonumentCommentId",
                principalTable: "MonumentComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonumentCommentLikes_AspNetUsers_UserId",
                table: "MonumentCommentLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonumentCommentLikes_MonumentComments_MonumentCommentId",
                table: "MonumentCommentLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_MonumentCommentLikes_AspNetUsers_UserId",
                table: "MonumentCommentLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonumentCommentLikes",
                table: "MonumentCommentLikes");

            migrationBuilder.RenameTable(
                name: "MonumentCommentLikes",
                newName: "MonumentCommentLike");

            migrationBuilder.RenameIndex(
                name: "IX_MonumentCommentLikes_UserId",
                table: "MonumentCommentLike",
                newName: "IX_MonumentCommentLike_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MonumentCommentLikes_MonumentCommentId",
                table: "MonumentCommentLike",
                newName: "IX_MonumentCommentLike_MonumentCommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonumentCommentLike",
                table: "MonumentCommentLike",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MonumentCommentLike_MonumentComments_MonumentCommentId",
                table: "MonumentCommentLike",
                column: "MonumentCommentId",
                principalTable: "MonumentComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonumentCommentLike_AspNetUsers_UserId",
                table: "MonumentCommentLike",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
