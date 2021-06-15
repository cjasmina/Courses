using Microsoft.EntityFrameworkCore.Migrations;

namespace Courses.Migrations
{
    public partial class DodanoPrisustva : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prisustva",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PredavanjeId = table.Column<int>(nullable: false),
                    KursKorisnikId = table.Column<int>(nullable: false),
                    Napomena = table.Column<string>(nullable: true),
                    Prisutan = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prisustva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prisustva_KursKorisnici_KursKorisnikId",
                        column: x => x.KursKorisnikId,
                        principalTable: "KursKorisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prisustva_Predavanja_PredavanjeId",
                        column: x => x.PredavanjeId,
                        principalTable: "Predavanja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prisustva_KursKorisnikId",
                table: "Prisustva",
                column: "KursKorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Prisustva_PredavanjeId",
                table: "Prisustva",
                column: "PredavanjeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prisustva");
        }
    }
}
