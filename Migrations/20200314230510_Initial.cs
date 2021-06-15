using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Courses.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drzave",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drzave", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Oblasti",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oblasti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoviObavijesti",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoviObavijesti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gradovi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(nullable: false),
                    PostanskiBroj = table.Column<string>(nullable: false),
                    DrzavaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gradovi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gradovi_Drzave_DrzavaId",
                        column: x => x.DrzavaId,
                        principalTable: "Drzave",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kursevi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ikona = table.Column<byte[]>(nullable: true),
                    Naziv = table.Column<string>(nullable: false),
                    Opis = table.Column<string>(nullable: false),
                    BrojSati = table.Column<int>(nullable: false),
                    Cijena = table.Column<int>(nullable: false),
                    DatumPocetka = table.Column<DateTime>(nullable: false),
                    DatumZavrsetka = table.Column<DateTime>(nullable: false),
                    OblastId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kursevi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kursevi_Oblasti_OblastId",
                        column: x => x.OblastId,
                        principalTable: "Oblasti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Obavijesti",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NaslovnaFotografija = table.Column<byte[]>(nullable: false),
                    Naziv = table.Column<string>(nullable: false),
                    Opis = table.Column<string>(nullable: false),
                    KratakOpis = table.Column<string>(nullable: false),
                    DatumObjave = table.Column<DateTime>(nullable: false),
                    DatumAzuriranja = table.Column<DateTime>(nullable: false),
                    Dodatak = table.Column<byte[]>(nullable: true),
                    DodatakNaziv = table.Column<string>(nullable: true),
                    TipObavijestiId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obavijesti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Obavijesti_TipoviObavijesti_TipObavijestiId",
                        column: x => x.TipObavijestiId,
                        principalTable: "TipoviObavijesti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfilnaFotografija = table.Column<byte[]>(nullable: true),
                    Ime = table.Column<string>(nullable: false),
                    Prezime = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    KorisnickoIme = table.Column<string>(nullable: false),
                    LozinkaHash = table.Column<string>(nullable: false),
                    LozinkaSalt = table.Column<string>(nullable: false),
                    Uloga = table.Column<int>(nullable: false),
                    DatumRegistracije = table.Column<DateTime>(nullable: false),
                    DatumPosljednjePrijave = table.Column<DateTime>(nullable: true),
                    GradId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Korisnici_Gradovi_GradId",
                        column: x => x.GradId,
                        principalTable: "Gradovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gradovi_DrzavaId",
                table: "Gradovi",
                column: "DrzavaId");

            migrationBuilder.CreateIndex(
                name: "IX_Korisnici_GradId",
                table: "Korisnici",
                column: "GradId");

            migrationBuilder.CreateIndex(
                name: "IX_Kursevi_OblastId",
                table: "Kursevi",
                column: "OblastId");

            migrationBuilder.CreateIndex(
                name: "IX_Obavijesti_TipObavijestiId",
                table: "Obavijesti",
                column: "TipObavijestiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Korisnici");

            migrationBuilder.DropTable(
                name: "Kursevi");

            migrationBuilder.DropTable(
                name: "Obavijesti");

            migrationBuilder.DropTable(
                name: "Gradovi");

            migrationBuilder.DropTable(
                name: "Oblasti");

            migrationBuilder.DropTable(
                name: "TipoviObavijesti");

            migrationBuilder.DropTable(
                name: "Drzave");
        }
    }
}
