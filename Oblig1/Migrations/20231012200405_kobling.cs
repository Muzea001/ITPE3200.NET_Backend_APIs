using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oblig1.Migrations
{
    /// <inheritdoc />
    public partial class kobling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "kundeID",
                table: "hus",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_hus_kundeID",
                table: "hus",
                column: "kundeID");

            migrationBuilder.AddForeignKey(
                name: "FK_hus_kunde_kundeID",
                table: "hus",
                column: "kundeID",
                principalTable: "kunde",
                principalColumn: "kundeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hus_kunde_kundeID",
                table: "hus");

            migrationBuilder.DropIndex(
                name: "IX_hus_kundeID",
                table: "hus");

            migrationBuilder.DropColumn(
                name: "kundeID",
                table: "hus");
        }
    }
}
