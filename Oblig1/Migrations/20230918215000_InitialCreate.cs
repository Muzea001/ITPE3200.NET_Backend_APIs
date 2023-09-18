using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oblig1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    personID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Navn = table.Column<string>(type: "TEXT", nullable: false),
                    Fodselsdato = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Addresse = table.Column<string>(type: "TEXT", nullable: false),
                    TelefonNmr = table.Column<long>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    personID1 = table.Column<int>(type: "INTEGER", nullable: true),
                    Passord = table.Column<string>(type: "TEXT", nullable: true),
                    erAdmin = table.Column<bool>(type: "INTEGER", nullable: true),
                    erEier = table.Column<bool>(type: "INTEGER", nullable: true),
                    eierID = table.Column<int>(type: "INTEGER", nullable: true),
                    brukerpersonID = table.Column<int>(type: "INTEGER", nullable: true),
                    kontoNummer = table.Column<long>(type: "INTEGER", nullable: true),
                    kundeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.personID);
                    table.ForeignKey(
                        name: "FK_person_person_brukerpersonID",
                        column: x => x.brukerpersonID,
                        principalTable: "person",
                        principalColumn: "personID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_person_person_personID1",
                        column: x => x.personID1,
                        principalTable: "person",
                        principalColumn: "personID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hus",
                columns: table => new
                {
                    husId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Beskrivelse = table.Column<string>(type: "TEXT", maxLength: 400, nullable: true),
                    areal = table.Column<double>(type: "REAL", nullable: false),
                    Pris = table.Column<decimal>(type: "TEXT", nullable: false),
                    by = table.Column<string>(type: "TEXT", nullable: false),
                    Addresse = table.Column<string>(type: "TEXT", nullable: false),
                    romAntall = table.Column<int>(type: "INTEGER", nullable: false),
                    erTilgjengelig = table.Column<bool>(type: "INTEGER", nullable: false),
                    eierID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hus", x => x.husId);
                    table.ForeignKey(
                        name: "FK_hus_person_eierID",
                        column: x => x.eierID,
                        principalTable: "person",
                        principalColumn: "personID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ordre",
                columns: table => new
                {
                    ordreId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Dato = table.Column<DateTime>(type: "TEXT", nullable: false),
                    betaltGjennom = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    husID = table.Column<int>(type: "INTEGER", nullable: false),
                    kundeID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ordre", x => x.ordreId);
                    table.ForeignKey(
                        name: "FK_ordre_hus_husID",
                        column: x => x.husID,
                        principalTable: "hus",
                        principalColumn: "husId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ordre_person_kundeID",
                        column: x => x.kundeID,
                        principalTable: "person",
                        principalColumn: "personID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_hus_eierID",
                table: "hus",
                column: "eierID");

            migrationBuilder.CreateIndex(
                name: "IX_ordre_husID",
                table: "ordre",
                column: "husID");

            migrationBuilder.CreateIndex(
                name: "IX_ordre_kundeID",
                table: "ordre",
                column: "kundeID");

            migrationBuilder.CreateIndex(
                name: "IX_person_brukerpersonID",
                table: "person",
                column: "brukerpersonID");

            migrationBuilder.CreateIndex(
                name: "IX_person_personID1",
                table: "person",
                column: "personID1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ordre");

            migrationBuilder.DropTable(
                name: "hus");

            migrationBuilder.DropTable(
                name: "person");
        }
    }
}
