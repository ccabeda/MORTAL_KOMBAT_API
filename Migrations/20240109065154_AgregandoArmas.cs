using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_MortalKombat.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoArmas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Armas",
                table: "Personajes");

            migrationBuilder.CreateTable(
                name: "Arma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arma", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArmaPersonaje",
                columns: table => new
                {
                    ArmasId = table.Column<int>(type: "int", nullable: false),
                    PersonajesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmaPersonaje", x => new { x.ArmasId, x.PersonajesId });
                    table.ForeignKey(
                        name: "FK_ArmaPersonaje_Arma_ArmasId",
                        column: x => x.ArmasId,
                        principalTable: "Arma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArmaPersonaje_Personajes_PersonajesId",
                        column: x => x.PersonajesId,
                        principalTable: "Personajes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Arma",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[] { 1, "Espada de hielo", "Kori Blade" });

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 3, 51, 53, 577, DateTimeKind.Local).AddTicks(5199));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 3, 51, 53, 577, DateTimeKind.Local).AddTicks(5234));

            migrationBuilder.UpdateData(
                table: "Personajes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 3, 51, 53, 577, DateTimeKind.Local).AddTicks(5268));

            migrationBuilder.UpdateData(
                table: "Reinos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 3, 51, 53, 577, DateTimeKind.Local).AddTicks(5248));

            migrationBuilder.CreateIndex(
                name: "IX_ArmaPersonaje_PersonajesId",
                table: "ArmaPersonaje",
                column: "PersonajesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArmaPersonaje");

            migrationBuilder.DropTable(
                name: "Arma");

            migrationBuilder.AddColumn<string>(
                name: "Armas",
                table: "Personajes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 3, 3, 51, 41, 885, DateTimeKind.Local).AddTicks(3445));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 3, 3, 51, 41, 885, DateTimeKind.Local).AddTicks(3484));

            migrationBuilder.UpdateData(
                table: "Personajes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Armas", "FechaCreacion" },
                values: new object[] { "[\"Kori Blade\"]", new DateTime(2024, 1, 3, 3, 51, 41, 885, DateTimeKind.Local).AddTicks(3547) });

            migrationBuilder.UpdateData(
                table: "Reinos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 3, 3, 51, 41, 885, DateTimeKind.Local).AddTicks(3499));
        }
    }
}
