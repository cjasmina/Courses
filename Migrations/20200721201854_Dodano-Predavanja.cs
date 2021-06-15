using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Courses.Migrations
{
    public partial class DodanoPredavanja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PredavanjeId",
                table: "KursKorisnici",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Predavanja",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(nullable: false),
                    Opis = table.Column<string>(nullable: false),
                    DatumPredavanja = table.Column<DateTime>(nullable: false),
                    KursId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predavanja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Predavanja_Kursevi_KursId",
                        column: x => x.KursId,
                        principalTable: "Kursevi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KursKorisnici_PredavanjeId",
                table: "KursKorisnici",
                column: "PredavanjeId");

            migrationBuilder.CreateIndex(
                name: "IX_Predavanja_KursId",
                table: "Predavanja",
                column: "KursId");

            migrationBuilder.AddForeignKey(
                name: "FK_KursKorisnici_Predavanja_PredavanjeId",
                table: "KursKorisnici",
                column: "PredavanjeId",
                principalTable: "Predavanja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KursKorisnici_Predavanja_PredavanjeId",
                table: "KursKorisnici");

            migrationBuilder.DropTable(
                name: "Predavanja");

            migrationBuilder.DropIndex(
                name: "IX_KursKorisnici_PredavanjeId",
                table: "KursKorisnici");

            migrationBuilder.DropColumn(
                name: "PredavanjeId",
                table: "KursKorisnici");
        }
    }
}
