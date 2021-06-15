using Microsoft.EntityFrameworkCore.Migrations;

namespace Courses.Migrations
{
    public partial class DodanoKursKorisnici : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KursKorisnici",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KursId = table.Column<int>(nullable: false),
                    KorisnikId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KursKorisnici", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KursKorisnici_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KursKorisnici_Kursevi_KursId",
                        column: x => x.KursId,
                        principalTable: "Kursevi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KursKorisnici_KorisnikId",
                table: "KursKorisnici",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_KursKorisnici_KursId",
                table: "KursKorisnici",
                column: "KursId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KursKorisnici");
        }
    }
}
