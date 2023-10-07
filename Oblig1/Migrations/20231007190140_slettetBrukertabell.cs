using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oblig1.Migrations
{
    /// <inheritdoc />
    public partial class slettetBrukertabell : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_eier_bruker_personID",
                table: "eier");

            migrationBuilder.DropForeignKey(
                name: "FK_kunde_person_personID1",
                table: "kunde");

            migrationBuilder.DropTable(
                name: "bruker");

            migrationBuilder.DropIndex(
                name: "IX_kunde_personID1",
                table: "kunde");

            migrationBuilder.DropColumn(
                name: "kundeId",
                table: "kunde");

            migrationBuilder.DropColumn(
                name: "personID1",
                table: "kunde");

            migrationBuilder.RenameColumn(
                name: "eierID",
                table: "eier",
                newName: "antallAnnonser");

            migrationBuilder.AddForeignKey(
                name: "FK_eier_person_personID",
                table: "eier",
                column: "personID",
                principalTable: "person",
                principalColumn: "personID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_eier_person_personID",
                table: "eier");

            migrationBuilder.RenameColumn(
                name: "antallAnnonser",
                table: "eier",
                newName: "eierID");

            migrationBuilder.AddColumn<int>(
                name: "kundeId",
                table: "kunde",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "personID1",
                table: "kunde",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "bruker",
                columns: table => new
                {
                    personID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Passord = table.Column<string>(type: "TEXT", nullable: false),
                    erAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    erEier = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bruker", x => x.personID);
                    table.ForeignKey(
                        name: "FK_bruker_person_personID",
                        column: x => x.personID,
                        principalTable: "person",
                        principalColumn: "personID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_kunde_personID1",
                table: "kunde",
                column: "personID1");

            migrationBuilder.AddForeignKey(
                name: "FK_eier_bruker_personID",
                table: "eier",
                column: "personID",
                principalTable: "bruker",
                principalColumn: "personID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_kunde_person_personID1",
                table: "kunde",
                column: "personID1",
                principalTable: "person",
                principalColumn: "personID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
