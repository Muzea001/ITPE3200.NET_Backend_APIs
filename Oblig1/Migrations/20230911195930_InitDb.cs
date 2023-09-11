using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oblig1.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hus",
                columns: table => new
                {
                    husId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Beskrivelse = table.Column<string>(type: "TEXT", maxLength: 400, nullable: false),
                    areal = table.Column<int>(type: "INTEGER", nullable: false),
                    pris = table.Column<decimal>(type: "TEXT", nullable: false),
                    addresse = table.Column<string>(type: "TEXT", nullable: false),
                    romAntall = table.Column<int>(type: "INTEGER", nullable: false),
                    erTilgjengelig = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hus", x => x.husId);
                });

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    navn = table.Column<string>(type: "TEXT", nullable: false),
                    addresse = table.Column<string>(type: "TEXT", nullable: false),
                    telefonNmr = table.Column<int>(type: "INTEGER", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    kundeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ordre",
                columns: table => new
                {
                    ordreId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Dato = table.Column<DateTime>(type: "TEXT", nullable: false),
                    husId = table.Column<int>(type: "INTEGER", nullable: false),
                    KundeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ordre", x => x.ordreId);
                    table.ForeignKey(
                        name: "FK_ordre_hus_husId",
                        column: x => x.husId,
                        principalTable: "hus",
                        principalColumn: "husId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ordre_person_KundeId",
                        column: x => x.KundeId,
                        principalTable: "person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ordre_husId",
                table: "ordre",
                column: "husId");

            migrationBuilder.CreateIndex(
                name: "IX_ordre_KundeId",
                table: "ordre",
                column: "KundeId");
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
