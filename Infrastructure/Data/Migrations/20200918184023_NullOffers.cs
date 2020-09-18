using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class NullOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_ApplicationUsers_CreatedById",
                table: "Offers");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_ApplicationUsers_CreatedById",
                table: "Offers",
                column: "CreatedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_ApplicationUsers_CreatedById",
                table: "Offers");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_ApplicationUsers_CreatedById",
                table: "Offers",
                column: "CreatedById",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
