using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_MortalKombat.Migrations
{
    /// <inheritdoc />
    public partial class fix_bugs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 2);

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

            migrationBuilder.InsertData(
                table: "EstilosDePeleas",
                columns: new[] { "Id", "Descripcion", "FechaActualizacion", "FechaCreacion", "Nombre" },
                values: new object[] { 1, "Arte Marcial", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 10, 6, 4, 17, 613, DateTimeKind.Local).AddTicks(9573), "Kung Fu" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EstilosDePeleas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 2, 45, 28, 104, DateTimeKind.Local).AddTicks(5922));

            migrationBuilder.InsertData(
                table: "Armas",
                columns: new[] { "Id", "Descripcion", "FechaActualizacion", "FechaCreacion", "Nombre" },
                values: new object[] { 2, "Arma afilada", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 10, 2, 45, 28, 104, DateTimeKind.Local).AddTicks(5935), "Kunai" });

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 2, 45, 28, 104, DateTimeKind.Local).AddTicks(5799));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 2, 45, 28, 104, DateTimeKind.Local).AddTicks(5867));

            migrationBuilder.UpdateData(
                table: "Personajes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 2, 45, 28, 104, DateTimeKind.Local).AddTicks(5909));

            migrationBuilder.UpdateData(
                table: "Reinos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 2, 45, 28, 104, DateTimeKind.Local).AddTicks(5890));
        }
    }
}
