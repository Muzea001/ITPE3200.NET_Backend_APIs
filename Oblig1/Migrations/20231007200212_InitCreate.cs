using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oblig1.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hus_person_personID",
                table: "hus");

            migrationBuilder.DropForeignKey(
                name: "FK_ordre_kunde_KundepersonID",
                table: "ordre");

            migrationBuilder.DropForeignKey(
                name: "FK_ordre_person_personID",
                table: "ordre");

            migrationBuilder.DropIndex(
                name: "IX_ordre_personID",
                table: "ordre");

            migrationBuilder.DropIndex(
                name: "IX_hus_personID",
                table: "hus");

            migrationBuilder.RenameColumn(
                name: "KundepersonID",
                table: "ordre",
                newName: "kundepersonID");

            migrationBuilder.RenameIndex(
                name: "IX_ordre_KundepersonID",
                table: "ordre",
                newName: "IX_ordre_kundepersonID");

            migrationBuilder.AlterColumn<int>(
                name: "kundepersonID",
                table: "ordre",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "eierpersonID",
                table: "hus",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_hus_eierpersonID",
                table: "hus",
                column: "eierpersonID");

            migrationBuilder.AddForeignKey(
                name: "FK_hus_eier_eierpersonID",
                table: "hus",
                column: "eierpersonID",
                principalTable: "eier",
                principalColumn: "personID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ordre_kunde_kundepersonID",
                table: "ordre",
                column: "kundepersonID",
                principalTable: "kunde",
                principalColumn: "personID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hus_eier_eierpersonID",
                table: "hus");

            migrationBuilder.DropForeignKey(
                name: "FK_ordre_kunde_kundepersonID",
                table: "ordre");

            migrationBuilder.DropIndex(
                name: "IX_hus_eierpersonID",
                table: "hus");

            migrationBuilder.DropColumn(
                name: "eierpersonID",
                table: "hus");

            migrationBuilder.RenameColumn(
                name: "kundepersonID",
                table: "ordre",
                newName: "KundepersonID");

            migrationBuilder.RenameIndex(
                name: "IX_ordre_kundepersonID",
                table: "ordre",
                newName: "IX_ordre_KundepersonID");

            migrationBuilder.AlterColumn<int>(
                name: "KundepersonID",
                table: "ordre",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_ordre_personID",
                table: "ordre",
                column: "personID");

            migrationBuilder.CreateIndex(
                name: "IX_hus_personID",
                table: "hus",
                column: "personID");

            migrationBuilder.AddForeignKey(
                name: "FK_hus_person_personID",
                table: "hus",
                column: "personID",
                principalTable: "person",
                principalColumn: "personID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ordre_kunde_KundepersonID",
                table: "ordre",
                column: "KundepersonID",
                principalTable: "kunde",
                principalColumn: "personID");

            migrationBuilder.AddForeignKey(
                name: "FK_ordre_person_personID",
                table: "ordre",
                column: "personID",
                principalTable: "person",
                principalColumn: "personID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
