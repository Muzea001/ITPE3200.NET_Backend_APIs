using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oblig1.Migrations
{
    /// <inheritdoc />
    public partial class foreignkeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_person_person_brukerpersonID",
                table: "person");

            migrationBuilder.DropForeignKey(
                name: "FK_person_person_personID1",
                table: "person");

            migrationBuilder.DropIndex(
                name: "IX_person_brukerpersonID",
                table: "person");

            migrationBuilder.DropIndex(
                name: "IX_person_personID1",
                table: "person");

            migrationBuilder.DropColumn(
                name: "brukerpersonID",
                table: "person");

            migrationBuilder.DropColumn(
                name: "personID1",
                table: "person");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "brukerpersonID",
                table: "person",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "personID1",
                table: "person",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_person_brukerpersonID",
                table: "person",
                column: "brukerpersonID");

            migrationBuilder.CreateIndex(
                name: "IX_person_personID1",
                table: "person",
                column: "personID1");

            migrationBuilder.AddForeignKey(
                name: "FK_person_person_brukerpersonID",
                table: "person",
                column: "brukerpersonID",
                principalTable: "person",
                principalColumn: "personID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_person_person_personID1",
                table: "person",
                column: "personID1",
                principalTable: "person",
                principalColumn: "personID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
