using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_MortalKombat.Migrations
{
    /// <inheritdoc />
    public partial class fix_bugsv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EstiloDePeleaPersonaje",
                columns: new[] { "EstilosDePeleasId", "PersonajesId" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 6, 12, 25, 539, DateTimeKind.Local).AddTicks(8727));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 6, 12, 25, 539, DateTimeKind.Local).AddTicks(8623));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 6, 12, 25, 539, DateTimeKind.Local).AddTicks(8667));

            migrationBuilder.UpdateData(
                table: "EstilosDePeleas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 6, 12, 25, 539, DateTimeKind.Local).AddTicks(8742));

            migrationBuilder.UpdateData(
                table: "Personajes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 6, 12, 25, 539, DateTimeKind.Local).AddTicks(8708));

            migrationBuilder.UpdateData(
                table: "Reinos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 6, 12, 25, 539, DateTimeKind.Local).AddTicks(8686));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 6, 4, 17, 613, DateTimeKind.Local).AddTicks(9559));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 6, 4, 17, 613, DateTimeKind.Local).AddTicks(9467));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 6, 4, 17, 613, DateTimeKind.Local).AddTicks(9506));

            migrationBuilder.UpdateData(
                table: "EstilosDePeleas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 6, 4, 17, 613, DateTimeKind.Local).AddTicks(9573));

            migrationBuilder.UpdateData(
                table: "Personajes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 6, 4, 17, 613, DateTimeKind.Local).AddTicks(9544));

            migrationBuilder.UpdateData(
                table: "Reinos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 6, 4, 17, 613, DateTimeKind.Local).AddTicks(9526));
        }
    }
}
