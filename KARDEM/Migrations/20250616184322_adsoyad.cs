using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KARDEM.Migrations
{
    /// <inheritdoc />
    public partial class adsoyad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IstinafBilgileri_DosyaId",
                table: "IstinafBilgileri");

            migrationBuilder.AddColumn<string>(
                name: "AdSoyad",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_IstinafBilgileri_DosyaId",
                table: "IstinafBilgileri",
                column: "DosyaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IstinafBilgileri_DosyaId",
                table: "IstinafBilgileri");

            migrationBuilder.DropColumn(
                name: "AdSoyad",
                table: "Kullanicilar");

            migrationBuilder.CreateIndex(
                name: "IX_IstinafBilgileri_DosyaId",
                table: "IstinafBilgileri",
                column: "DosyaId",
                unique: true);
        }
    }
}
