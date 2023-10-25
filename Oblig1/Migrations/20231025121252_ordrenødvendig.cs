using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oblig1.Migrations
{
    /// <inheritdoc />
    public partial class ordrenødvendig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordre_Hus_husId",
                table: "Ordre");

            migrationBuilder.AlterColumn<int>(
                name: "husId",
                table: "Ordre",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ordre_Hus_husId",
                table: "Ordre",
                column: "husId",
                principalTable: "Hus",
                principalColumn: "husId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordre_Hus_husId",
                table: "Ordre");

            migrationBuilder.AlterColumn<int>(
                name: "husId",
                table: "Ordre",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Ordre_Hus_husId",
                table: "Ordre",
                column: "husId",
                principalTable: "Hus",
                principalColumn: "husId");
        }
    }
}
