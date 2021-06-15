using Microsoft.EntityFrameworkCore.Migrations;

namespace Courses.Migrations
{
    public partial class DodanoKursUObavijest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KursId",
                table: "Obavijesti",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Obavijesti_KursId",
                table: "Obavijesti",
                column: "KursId");

            migrationBuilder.AddForeignKey(
                name: "FK_Obavijesti_Kursevi_KursId",
                table: "Obavijesti",
                column: "KursId",
                principalTable: "Kursevi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Obavijesti_Kursevi_KursId",
                table: "Obavijesti");

            migrationBuilder.DropIndex(
                name: "IX_Obavijesti_KursId",
                table: "Obavijesti");

            migrationBuilder.DropColumn(
                name: "KursId",
                table: "Obavijesti");
        }
    }
}
