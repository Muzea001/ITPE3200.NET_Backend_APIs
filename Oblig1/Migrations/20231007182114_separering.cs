using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oblig1.Migrations
{
    /// <inheritdoc />
    public partial class separering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hus_person_eierID",
                table: "hus");

            migrationBuilder.DropForeignKey(
                name: "FK_ordre_kunde_kundeID",
                table: "ordre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kunde",
                table: "kunde");

            migrationBuilder.DropIndex(
                name: "IX_kunde_personID",
                table: "kunde");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "person");

            migrationBuilder.DropColumn(
                name: "Passord",
                table: "person");

            migrationBuilder.DropColumn(
                name: "eierID",
                table: "person");

            migrationBuilder.DropColumn(
                name: "erAdmin",
                table: "person");

            migrationBuilder.DropColumn(
                name: "erEier",
                table: "person");

            migrationBuilder.DropColumn(
                name: "kontoNummer",
                table: "person");

            migrationBuilder.AlterColumn<int>(
                name: "personID",
                table: "kunde",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "kundeId",
                table: "kunde",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "personID1",
                table: "kunde",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_kunde",
                table: "kunde",
                column: "personID");

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

            migrationBuilder.CreateTable(
                name: "eier",
                columns: table => new
                {
                    personID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    eierID = table.Column<int>(type: "INTEGER", nullable: false),
                    kontoNummer = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eier", x => x.personID);
                    table.ForeignKey(
                        name: "FK_eier_bruker_personID",
                        column: x => x.personID,
                        principalTable: "bruker",
                        principalColumn: "personID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_kunde_personID1",
                table: "kunde",
                column: "personID1");

            migrationBuilder.AddForeignKey(
                name: "FK_hus_eier_eierID",
                table: "hus",
                column: "eierID",
                principalTable: "eier",
                principalColumn: "personID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_kunde_person_personID1",
                table: "kunde",
                column: "personID1",
                principalTable: "person",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hus_eier_eierID",
                table: "hus");

            migrationBuilder.DropForeignKey(
                name: "FK_kunde_person_personID1",
                table: "kunde");

            migrationBuilder.DropForeignKey(
                name: "FK_ordre_kunde_kundeID",
                table: "ordre");

            migrationBuilder.DropTable(
                name: "eier");

            migrationBuilder.DropTable(
                name: "bruker");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kunde",
                table: "kunde");

            migrationBuilder.DropIndex(
                name: "IX_kunde_personID1",
                table: "kunde");

            migrationBuilder.DropColumn(
                name: "personID1",
                table: "kunde");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "person",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Passord",
                table: "person",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "eierID",
                table: "person",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "erAdmin",
                table: "person",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "erEier",
                table: "person",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "kontoNummer",
                table: "person",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "kundeId",
                table: "kunde",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "personID",
                table: "kunde",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_kunde",
                table: "kunde",
                column: "kundeId");

            migrationBuilder.CreateIndex(
                name: "IX_kunde_personID",
                table: "kunde",
                column: "personID");

            migrationBuilder.AddForeignKey(
                name: "FK_hus_person_eierID",
                table: "hus",
                column: "eierID",
                principalTable: "person",
                principalColumn: "personID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ordre_kunde_kundeID",
                table: "ordre",
                column: "kundeID",
                principalTable: "kunde",
                principalColumn: "kundeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
