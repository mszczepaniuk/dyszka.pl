using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class CommentsDeleting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ApplicationUsers_CreatedById",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ApplicationUsers_CreatedById",
                table: "Comments",
                column: "CreatedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ApplicationUsers_CreatedById",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ApplicationUsers_CreatedById",
                table: "Comments",
                column: "CreatedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
