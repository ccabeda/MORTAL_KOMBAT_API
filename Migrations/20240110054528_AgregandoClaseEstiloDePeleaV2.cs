using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_MortalKombat.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoClaseEstiloDePeleaV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstiloDePeleaPersonaje_EstilosDePeleas_EstilosDePeleaId",
                table: "EstiloDePeleaPersonaje");

            migrationBuilder.RenameColumn(
                name: "EstilosDePeleaId",
                table: "EstiloDePeleaPersonaje",
                newName: "EstilosDePeleasId");

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 2, 45, 28, 104, DateTimeKind.Local).AddTicks(5922));

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 10, 2, 45, 28, 104, DateTimeKind.Local).AddTicks(5935));

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

            migrationBuilder.AddForeignKey(
                name: "FK_EstiloDePeleaPersonaje_EstilosDePeleas_EstilosDePeleasId",
                table: "EstiloDePeleaPersonaje",
                column: "EstilosDePeleasId",
                principalTable: "EstilosDePeleas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstiloDePeleaPersonaje_EstilosDePeleas_EstilosDePeleasId",
                table: "EstiloDePeleaPersonaje");

            migrationBuilder.RenameColumn(
                name: "EstilosDePeleasId",
                table: "EstiloDePeleaPersonaje",
                newName: "EstilosDePeleaId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_EstiloDePeleaPersonaje_EstilosDePeleas_EstilosDePeleaId",
                table: "EstiloDePeleaPersonaje",
                column: "EstilosDePeleaId",
                principalTable: "EstilosDePeleas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
