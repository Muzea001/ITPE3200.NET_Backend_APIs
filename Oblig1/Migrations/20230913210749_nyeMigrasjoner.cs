using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oblig1.Migrations
{
    /// <inheritdoc />
    public partial class nyeMigrasjoner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "telefonNmr",
                table: "person",
                newName: "TelefonNmr");

            migrationBuilder.RenameColumn(
                name: "navn",
                table: "person",
                newName: "Navn");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "person",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "addresse",
                table: "person",
                newName: "Addresse");

            migrationBuilder.RenameColumn(
                name: "pris",
                table: "hus",
                newName: "Pris");

            migrationBuilder.RenameColumn(
                name: "addresse",
                table: "hus",
                newName: "Addresse");

            migrationBuilder.AddColumn<DateTime>(
                name: "Fodselsdato",
                table: "person",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "betaltGjennom",
                table: "ordre",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<double>(
                name: "areal",
                table: "hus",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<long>(
                name: "EeierkontoNummer",
                table: "hus",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Eeier",
                columns: table => new
                {
                    kontoNummer = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    brukerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eeier", x => x.kontoNummer);
                    table.ForeignKey(
                        name: "FK_Eeier_person_brukerId",
                        column: x => x.brukerId,
                        principalTable: "person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_hus_EeierkontoNummer",
                table: "hus",
                column: "EeierkontoNummer");

            migrationBuilder.CreateIndex(
                name: "IX_Eeier_brukerId",
                table: "Eeier",
                column: "brukerId");

            migrationBuilder.AddForeignKey(
                name: "FK_hus_Eeier_EeierkontoNummer",
                table: "hus",
                column: "EeierkontoNummer",
                principalTable: "Eeier",
                principalColumn: "kontoNummer",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hus_Eeier_EeierkontoNummer",
                table: "hus");

            migrationBuilder.DropTable(
                name: "Eeier");

            migrationBuilder.DropIndex(
                name: "IX_hus_EeierkontoNummer",
                table: "hus");

            migrationBuilder.DropColumn(
                name: "Fodselsdato",
                table: "person");

            migrationBuilder.DropColumn(
                name: "betaltGjennom",
                table: "ordre");

            migrationBuilder.DropColumn(
                name: "EeierkontoNummer",
                table: "hus");

            migrationBuilder.RenameColumn(
                name: "TelefonNmr",
                table: "person",
                newName: "telefonNmr");

            migrationBuilder.RenameColumn(
                name: "Navn",
                table: "person",
                newName: "navn");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "person",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Addresse",
                table: "person",
                newName: "addresse");

            migrationBuilder.RenameColumn(
                name: "Pris",
                table: "hus",
                newName: "pris");

            migrationBuilder.RenameColumn(
                name: "Addresse",
                table: "hus",
                newName: "addresse");

            migrationBuilder.AlterColumn<int>(
                name: "areal",
                table: "hus",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }
    }
}
