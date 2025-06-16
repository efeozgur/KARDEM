using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KARDEM.Migrations
{
    /// <inheritdoc />
    public partial class herikitaraf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KarsilikliTebligTarihi",
                table: "IstinafBilgileri");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "KarsilikliTebligTarihi",
                table: "IstinafBilgileri",
                type: "datetime2",
                nullable: true);
        }
    }
}
