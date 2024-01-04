using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_MortalKombat.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoHasData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Armas",
                table: "Personajes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EstilosDePelea",
                table: "Personajes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Clanes",
                columns: new[] { "Id", "Descripcion", "FechaActualizacion", "FechaCreacion", "Nombre" },
                values: new object[] { 1, "Lin Kuei, el clan de fuertes guerreros Ninja chinos, es una sociedad ubicada al norte de Asia, que mata por dinero desde muchas generaciones atrás. Sus integrantes son instruídos desde muy temprana edad, y apenas tienen contacto con el mundo exterior. Los mejores guerreros del clan tienen algún poder especial, y cuando llegan a dominarlo, reciben un nombre clave. Se descubrió que solo los que vienen de un linaje antiguo, llegan a dominar este poder.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 3, 1, 27, 16, 599, DateTimeKind.Local).AddTicks(2492), "Lin Kuei" });

            migrationBuilder.InsertData(
                table: "Reinos",
                columns: new[] { "Id", "Descripcion", "FechaActualizacion", "FechaCreacion", "Nombre" },
                values: new object[] { 1, "La tierra es uno de los muchos reinos que crearon los antiguos dioses, dicho reino ha sido objeto frecuente de intentos de conquistas, porque se piensa que es la joya del cosmos y un importante centro de energía universal. Debido a esto, los dioses han designado una deidad para proteger el reino de daños, por innumerables siglos éste ha sido Raiden, aunque también otros dioses como Fujin, desempeñan un papel en esta labor.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 3, 1, 27, 16, 599, DateTimeKind.Local).AddTicks(2636), "Tierra" });

            migrationBuilder.InsertData(
                table: "Personajes",
                columns: new[] { "Id", "Alineacion", "Armas", "ClanId", "Descripcion", "EstilosDePelea", "FechaActualizacion", "FechaCreacion", "ImagenURl", "Nombre", "Raza", "ReinoId" },
                values: new object[] { 1, "Bien", "[\"Kori Blade\"]", 1, "Hermano menor del Sub-Zero original, comenzó reemplazando a su hermano caído y terminó convirtiéndose en el nuevo Gran Maestro Lin Kuei. Como descendiente de los cryomancers, conserva las habilidades para congelar.", "[\"Shotokan\",\"Drag\\u00F3n\"]", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 3, 1, 27, 16, 599, DateTimeKind.Local).AddTicks(2661), "", "Sub-Zero", "Humano/Cryomancer", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Personajes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reinos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Armas",
                table: "Personajes");

            migrationBuilder.DropColumn(
                name: "EstilosDePelea",
                table: "Personajes");
        }
    }
}
