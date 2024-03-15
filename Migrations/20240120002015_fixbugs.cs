using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_MortalKombat.Migrations
{
    /// <inheritdoc />
    public partial class Fixbugs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaDeCreación",
                table: "Usuarios",
                newName: "FechaCreacion");

            migrationBuilder.RenameColumn(
                name: "FechaDeActualización",
                table: "Usuarios",
                newName: "FechaActualizacion");

            migrationBuilder.RenameColumn(
                name: "FechaDeCreación",
                table: "Roles",
                newName: "FechaCreacion");

            migrationBuilder.RenameColumn(
                name: "FechaDeActualización",
                table: "Roles",
                newName: "FechaActualizacion");

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 19, 21, 20, 15, 314, DateTimeKind.Local).AddTicks(3811));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 19, 21, 20, 15, 314, DateTimeKind.Local).AddTicks(3715));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 19, 21, 20, 15, 314, DateTimeKind.Local).AddTicks(3754));

            migrationBuilder.UpdateData(
                table: "EstilosDePeleas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 19, 21, 20, 15, 314, DateTimeKind.Local).AddTicks(3826));

            migrationBuilder.UpdateData(
                table: "Personajes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 19, 21, 20, 15, 314, DateTimeKind.Local).AddTicks(3794));

            migrationBuilder.UpdateData(
                table: "Reinos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 19, 21, 20, 15, 314, DateTimeKind.Local).AddTicks(3775));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 19, 21, 20, 15, 314, DateTimeKind.Local).AddTicks(3912));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 19, 21, 20, 15, 314, DateTimeKind.Local).AddTicks(3928));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 19, 21, 20, 15, 314, DateTimeKind.Local).AddTicks(3941));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "Usuarios",
                newName: "FechaDeCreación");

            migrationBuilder.RenameColumn(
                name: "FechaActualizacion",
                table: "Usuarios",
                newName: "FechaDeActualización");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "Roles",
                newName: "FechaDeCreación");

            migrationBuilder.RenameColumn(
                name: "FechaActualizacion",
                table: "Roles",
                newName: "FechaDeActualización");

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 18, 22, 31, 3, 112, DateTimeKind.Local).AddTicks(5764));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 18, 22, 31, 3, 112, DateTimeKind.Local).AddTicks(5675));

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 18, 22, 31, 3, 112, DateTimeKind.Local).AddTicks(5711));

            migrationBuilder.UpdateData(
                table: "EstilosDePeleas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 18, 22, 31, 3, 112, DateTimeKind.Local).AddTicks(5780));

            migrationBuilder.UpdateData(
                table: "Personajes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 18, 22, 31, 3, 112, DateTimeKind.Local).AddTicks(5750));

            migrationBuilder.UpdateData(
                table: "Reinos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 18, 22, 31, 3, 112, DateTimeKind.Local).AddTicks(5729));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaDeCreación",
                value: new DateTime(2024, 1, 18, 22, 31, 3, 112, DateTimeKind.Local).AddTicks(5832));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaDeCreación",
                value: new DateTime(2024, 1, 18, 22, 31, 3, 112, DateTimeKind.Local).AddTicks(5864));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaDeCreación",
                value: new DateTime(2024, 1, 18, 22, 31, 3, 112, DateTimeKind.Local).AddTicks(5878));
        }
    }
}
