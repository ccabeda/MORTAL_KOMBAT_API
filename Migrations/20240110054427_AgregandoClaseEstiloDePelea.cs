using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_MortalKombat.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoClaseEstiloDePelea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstilosDePelea",
                table: "Personajes");

            migrationBuilder.CreateTable(
                name: "EstilosDePeleas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstilosDePeleas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstiloDePeleaPersonaje",
                columns: table => new
                {
                    EstilosDePeleaId = table.Column<int>(type: "int", nullable: false),
                    PersonajesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstiloDePeleaPersonaje", x => new { x.EstilosDePeleaId, x.PersonajesId });
                    table.ForeignKey(
                        name: "FK_EstiloDePeleaPersonaje_EstilosDePeleas_EstilosDePeleaId",
                        column: x => x.EstilosDePeleaId,
                        principalTable: "EstilosDePeleas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstiloDePeleaPersonaje_Personajes_PersonajesId",
                        column: x => x.PersonajesId,
                        principalTable: "Personajes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 2, 44, 27, 177, DateTimeKind.Local).AddTicks(7950));

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 2, 44, 27, 177, DateTimeKind.Local).AddTicks(7961));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 2, 44, 27, 177, DateTimeKind.Local).AddTicks(7863));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 2, 44, 27, 177, DateTimeKind.Local).AddTicks(7899));

            migrationBuilder.UpdateData(
                table: "Personajes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 2, 44, 27, 177, DateTimeKind.Local).AddTicks(7935));

            migrationBuilder.UpdateData(
                table: "Reinos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 2, 44, 27, 177, DateTimeKind.Local).AddTicks(7915));

            migrationBuilder.CreateIndex(
                name: "IX_EstiloDePeleaPersonaje_PersonajesId",
                table: "EstiloDePeleaPersonaje",
                column: "PersonajesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstiloDePeleaPersonaje");

            migrationBuilder.DropTable(
                name: "EstilosDePeleas");

            migrationBuilder.AddColumn<string>(
                name: "EstilosDePelea",
                table: "Personajes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 6, 46, 46, 727, DateTimeKind.Local).AddTicks(2186));

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 6, 46, 46, 727, DateTimeKind.Local).AddTicks(2223));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 6, 46, 46, 727, DateTimeKind.Local).AddTicks(2104));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 6, 46, 46, 727, DateTimeKind.Local).AddTicks(2137));

            migrationBuilder.UpdateData(
                table: "Personajes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EstilosDePelea", "FechaCreacion" },
                values: new object[] { "[\"Shotokan\",\"Drag\\u00F3n\"]", new DateTime(2024, 1, 9, 6, 46, 46, 727, DateTimeKind.Local).AddTicks(2172) });

            migrationBuilder.UpdateData(
                table: "Reinos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 6, 46, 46, 727, DateTimeKind.Local).AddTicks(2152));
        }
    }
}
