using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KARDEM.Migrations
{
    /// <inheritdoc />
    public partial class harcbilgi2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HarcBilgileri_DosyaId1",
                table: "HarcBilgileri");

            migrationBuilder.CreateIndex(
                name: "IX_HarcBilgileri_DosyaId1",
                table: "HarcBilgileri",
                column: "DosyaId1",
                unique: true,
                filter: "[DosyaId1] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HarcBilgileri_DosyaId1",
                table: "HarcBilgileri");

            migrationBuilder.CreateIndex(
                name: "IX_HarcBilgileri_DosyaId1",
                table: "HarcBilgileri",
                column: "DosyaId1");
        }
    }
}
