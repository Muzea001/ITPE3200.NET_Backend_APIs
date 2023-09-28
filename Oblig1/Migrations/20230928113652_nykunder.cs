using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oblig1.Migrations
{
    /// <inheritdoc />
    public partial class nykunder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ordre_person_kundeID",
                table: "ordre");

            migrationBuilder.DropColumn(
                name: "kundeId",
                table: "person");

            migrationBuilder.CreateTable(
                name: "kunde",
                columns: table => new
                {
                    kundeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    personID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kunde", x => x.kundeId);
                    table.ForeignKey(
                        name: "FK_kunde_person_personID",
                        column: x => x.personID,
                        principalTable: "person",
                        principalColumn: "personID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_kunde_personID",
                table: "kunde",
                column: "personID");

            migrationBuilder.AddForeignKey(
                name: "FK_ordre_kunde_kundeID",
                table: "ordre",
                column: "kundeID",
                principalTable: "kunde",
                principalColumn: "kundeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ordre_kunde_kundeID",
                table: "ordre");

            migrationBuilder.DropTable(
                name: "kunde");

            migrationBuilder.AddColumn<int>(
                name: "kundeId",
                table: "person",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ordre_person_kundeID",
                table: "ordre",
                column: "kundeID",
                principalTable: "person",
                principalColumn: "personID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
