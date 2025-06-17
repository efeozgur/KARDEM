using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KARDEM.Migrations
{
    /// <inheritdoc />
    public partial class HarcBilgileri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DosyaId1",
                table: "HarcBilgileri",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HarcBilgileri_DosyaId1",
                table: "HarcBilgileri",
                column: "DosyaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HarcBilgileri_Dosyalar_DosyaId1",
                table: "HarcBilgileri",
                column: "DosyaId1",
                principalTable: "Dosyalar",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HarcBilgileri_Dosyalar_DosyaId1",
                table: "HarcBilgileri");

            migrationBuilder.DropIndex(
                name: "IX_HarcBilgileri_DosyaId1",
                table: "HarcBilgileri");

            migrationBuilder.DropColumn(
                name: "DosyaId1",
                table: "HarcBilgileri");
        }
    }
}
