using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_MortalKombat.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoNuevoClanV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 3, 3, 51, 41, 885, DateTimeKind.Local).AddTicks(3445));

            migrationBuilder.InsertData(
                table: "Clanes",
                columns: new[] { "Id", "Descripcion", "FechaActualizacion", "FechaCreacion", "Nombre" },
                values: new object[] { 2, "El clan Shirai Ryu se formó hace muchos años por obra de un guerrero Lin Kuei llamado Takeda, quien abandonó su clan. Al abandonar el Lin Kuei,Takeda se convirtió en un ninja desertor,crimen que se castiga con la muerte.Takeda fue buscado por los asesinos Lin Kuei.Escapó de China y regresó a Japón,su patria,donde ofreció servicios a los señores y generales.Su técnica gradualmente se extendió por todo Japón, y se convirtió en el arte del Ninjutsu.Además de enseñar su nueva forma de arte marcial,también enseñaba versiones modificadas de las tácticas Lin Kuei,de modo que reveló muchos de sus secretos.Esto avivó la furia de los Lin Kuei.Las enseñanzas de Takeda se hicieron conocidas en todo el Japón por obra de sus muchos seguidores,los cuales pasaron a ser conocidos como el clan Shirai Ryu.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 3, 3, 51, 41, 885, DateTimeKind.Local).AddTicks(3484), "Shirai Ryu" });

            migrationBuilder.UpdateData(
                table: "Personajes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 3, 3, 51, 41, 885, DateTimeKind.Local).AddTicks(3547));

            migrationBuilder.UpdateData(
                table: "Reinos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 3, 3, 51, 41, 885, DateTimeKind.Local).AddTicks(3499));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Clanes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 3, 3, 50, 54, 43, DateTimeKind.Local).AddTicks(1128));

            migrationBuilder.InsertData(
                table: "Clanes",
                columns: new[] { "Id", "Descripcion", "FechaActualizacion", "FechaCreacion", "Nombre" },
                values: new object[] { 12, "El clan Shirai Ryu se formó hace muchos años por obra de un guerrero Lin Kuei llamado Takeda, quien abandonó su clan. Al abandonar el Lin Kuei,Takeda se convirtió en un ninja desertor,crimen que se castiga con la muerte.Takeda fue buscado por los asesinos Lin Kuei.Escapó de China y regresó a Japón,su patria,donde ofreció servicios a los señores y generales.Su técnica gradualmente se extendió por todo Japón, y se convirtió en el arte del Ninjutsu.Además de enseñar su nueva forma de arte marcial,también enseñaba versiones modificadas de las tácticas Lin Kuei,de modo que reveló muchos de sus secretos.Esto avivó la furia de los Lin Kuei.Las enseñanzas de Takeda se hicieron conocidas en todo el Japón por obra de sus muchos seguidores,los cuales pasaron a ser conocidos como el clan Shirai Ryu.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 3, 3, 50, 54, 43, DateTimeKind.Local).AddTicks(1167), "Shirai Ryu" });

            migrationBuilder.UpdateData(
                table: "Personajes",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 3, 3, 50, 54, 43, DateTimeKind.Local).AddTicks(1234));

            migrationBuilder.UpdateData(
                table: "Reinos",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaCreacion",
                value: new DateTime(2024, 1, 3, 3, 50, 54, 43, DateTimeKind.Local).AddTicks(1184));
        }
    }
}
