using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_MortalKombat.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoArmaPersonaje : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "ArmaPersonaje",
                columns: new[] { "ArmasId", "PersonajesId" },
                values: new object[] { 1, 1});
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
