using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oblig1.Migrations
{
    /// <inheritdoc />
    public partial class nullableLists2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bilder_Hus_husId",
                table: "Bilder");

            migrationBuilder.AlterColumn<int>(
                name: "husId",
                table: "Bilder",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Bilder_Hus_husId",
                table: "Bilder",
                column: "husId",
                principalTable: "Hus",
                principalColumn: "husId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bilder_Hus_husId",
                table: "Bilder");

            migrationBuilder.AlterColumn<int>(
                name: "husId",
                table: "Bilder",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bilder_Hus_husId",
                table: "Bilder",
                column: "husId",
                principalTable: "Hus",
                principalColumn: "husId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
