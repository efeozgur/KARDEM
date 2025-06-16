using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KARDEM.Migrations
{
    /// <inheritdoc />
    public partial class istinafbilgileri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IstinafBilgileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosyaId = table.Column<int>(type: "int", nullable: false),
                    IstinafEdenTaraf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IstinafTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DilekceTebligTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KarsilikliTebligTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SureKontroluYapildiMi = table.Column<bool>(type: "bit", nullable: false),
                    DosyaGonderildiMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IstinafBilgileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IstinafBilgileri_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IstinafBilgileri_DosyaId",
                table: "IstinafBilgileri",
                column: "DosyaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IstinafBilgileri");
        }
    }
}
