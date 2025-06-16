using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KARDEM.Migrations
{
    /// <inheritdoc />
    public partial class Kurulum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sifre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mahkemeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KesinlesmeSuresiGun = table.Column<int>(type: "int", nullable: false),
                    IstinafSuresiGun = table.Column<int>(type: "int", nullable: false),
                    TemyizSuresiGun = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahkemeler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dosyalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EsasNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KararNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KararTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KararTebligTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DosyaNotu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MahkemeId = table.Column<int>(type: "int", nullable: false),
                    EkleyenKullaniciId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dosyalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dosyalar_Kullanicilar_EkleyenKullaniciId",
                        column: x => x.EkleyenKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Dosyalar_Mahkemeler_MahkemeId",
                        column: x => x.MahkemeId,
                        principalTable: "Mahkemeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MahkemeYetkileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    MahkemeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MahkemeYetkileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MahkemeYetkileri_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MahkemeYetkileri_Mahkemeler_MahkemeId",
                        column: x => x.MahkemeId,
                        principalTable: "Mahkemeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dosyalar_EkleyenKullaniciId",
                table: "Dosyalar",
                column: "EkleyenKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Dosyalar_MahkemeId",
                table: "Dosyalar",
                column: "MahkemeId");

            migrationBuilder.CreateIndex(
                name: "IX_MahkemeYetkileri_KullaniciId",
                table: "MahkemeYetkileri",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_MahkemeYetkileri_MahkemeId",
                table: "MahkemeYetkileri",
                column: "MahkemeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dosyalar");

            migrationBuilder.DropTable(
                name: "MahkemeYetkileri");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Mahkemeler");
        }
    }
}
