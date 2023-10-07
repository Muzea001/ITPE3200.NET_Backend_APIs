using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oblig1.Migrations
{
    /// <inheritdoc />
    public partial class nøkkler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hus_eier_eierID",
                table: "hus");

            migrationBuilder.DropForeignKey(
                name: "FK_ordre_kunde_kundeID",
                table: "ordre");

            migrationBuilder.RenameColumn(
                name: "kundeID",
                table: "ordre",
                newName: "personID");

            migrationBuilder.RenameIndex(
                name: "IX_ordre_kundeID",
                table: "ordre",
                newName: "IX_ordre_personID");

            migrationBuilder.RenameColumn(
                name: "eierID",
                table: "hus",
                newName: "personID");

            migrationBuilder.RenameIndex(
                name: "IX_hus_eierID",
                table: "hus",
                newName: "IX_hus_personID");

            migrationBuilder.AddColumn<int>(
                name: "KundepersonID",
                table: "ordre",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ordre_KundepersonID",
                table: "ordre",
                column: "KundepersonID");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "IX_ordre_KundepersonID",
                table: "ordre");

            migrationBuilder.DropColumn(
                name: "KundepersonID",
                table: "ordre");

            migrationBuilder.RenameColumn(
                name: "personID",
                table: "ordre",
                newName: "kundeID");

            migrationBuilder.RenameIndex(
                name: "IX_ordre_personID",
                table: "ordre",
                newName: "IX_ordre_kundeID");

            migrationBuilder.RenameColumn(
                name: "personID",
                table: "hus",
                newName: "eierID");

            migrationBuilder.RenameIndex(
                name: "IX_hus_personID",
                table: "hus",
                newName: "IX_hus_eierID");

            migrationBuilder.AddForeignKey(
                name: "FK_hus_eier_eierID",
                table: "hus",
                column: "eierID",
                principalTable: "eier",
                principalColumn: "personID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ordre_kunde_kundeID",
                table: "ordre",
                column: "kundeID",
                principalTable: "kunde",
                principalColumn: "personID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
