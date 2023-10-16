using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oblig1.Migrations
{
    /// <inheritdoc />
    public partial class nullableOrdreListe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bilder_hus_husId",
                table: "bilder");

            migrationBuilder.AlterColumn<int>(
                name: "husId",
                table: "bilder",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_bilder_hus_husId",
                table: "bilder",
                column: "husId",
                principalTable: "hus",
                principalColumn: "husId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bilder_hus_husId",
                table: "bilder");

            migrationBuilder.AlterColumn<int>(
                name: "husId",
                table: "bilder",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_bilder_hus_husId",
                table: "bilder",
                column: "husId",
                principalTable: "hus",
                principalColumn: "husId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
