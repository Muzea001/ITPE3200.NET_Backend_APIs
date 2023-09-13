using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oblig1.Migrations
{
    /// <inheritdoc />
    public partial class oppdaterHus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Beskrivelse",
                table: "hus",
                type: "TEXT",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 400);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Beskrivelse",
                table: "hus",
                type: "TEXT",
                maxLength: 400,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 400,
                oldNullable: true);
        }
    }
}
