using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KARDEM.Migrations
{
    /// <inheritdoc />
    public partial class HarcIslem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HarcBilgileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HarcTuru = table.Column<int>(type: "int", nullable: false),
                    HarcinTarafi = table.Column<int>(type: "int", nullable: false),
                    HarcMiktari = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HarcYazilacakTarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YazilmaDurumu = table.Column<bool>(type: "bit", nullable: false),
                    DosyaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HarcBilgileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HarcBilgileri_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HarcBilgileri_DosyaId",
                table: "HarcBilgileri",
                column: "DosyaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HarcBilgileri");
        }
    }
}
