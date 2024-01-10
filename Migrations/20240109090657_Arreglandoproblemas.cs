using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_MortalKombat.Migrations
{
    /// <inheritdoc />
    public partial class Arreglandoproblemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArmaPersonaje_Arma_ArmasId",
                table: "ArmaPersonaje");

            migrationBuilder.DropTable(
                name: "Arma");

            migrationBuilder.CreateTable(
                name: "Armas",
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
                    table.PrimaryKey("PK_Armas", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Armas",
                columns: new[] { "Id", "Descripcion", "FechaActualizacion", "FechaCreacion", "Nombre" },
                values: new object[] { 1, "Espada de hielo", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kori Blade" });

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 6, 6, 56, 749, DateTimeKind.Local).AddTicks(5436));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 6, 6, 56, 749, DateTimeKind.Local).AddTicks(5475));

            migrationBuilder.UpdateData(
                table: "Personajes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 6, 6, 56, 749, DateTimeKind.Local).AddTicks(5549));

            migrationBuilder.UpdateData(
                table: "Reinos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 6, 6, 56, 749, DateTimeKind.Local).AddTicks(5525));

            migrationBuilder.AddForeignKey(
                name: "FK_ArmaPersonaje_Armas_ArmasId",
                table: "ArmaPersonaje",
                column: "ArmasId",
                principalTable: "Armas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArmaPersonaje_Armas_ArmasId",
                table: "ArmaPersonaje");

            migrationBuilder.DropTable(
                name: "Armas");

            migrationBuilder.CreateTable(
                name: "Arma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arma", x => x.Id);
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
                value: new DateTime(2024, 1, 9, 3, 55, 24, 559, DateTimeKind.Local).AddTicks(6407));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 3, 55, 24, 559, DateTimeKind.Local).AddTicks(6442));

            migrationBuilder.UpdateData(
                table: "Personajes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 3, 55, 24, 559, DateTimeKind.Local).AddTicks(6481));

            migrationBuilder.UpdateData(
                table: "Reinos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 9, 3, 55, 24, 559, DateTimeKind.Local).AddTicks(6458));

            migrationBuilder.AddForeignKey(
                name: "FK_ArmaPersonaje_Arma_ArmasId",
                table: "ArmaPersonaje",
                column: "ArmasId",
                principalTable: "Arma",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
