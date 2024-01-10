﻿// <auto-generated />
using System;
using API_MortalKombat.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API_MortalKombat.Migrations
{
    [DbContext(typeof(AplicationDbContext))]
    [Migration("20240110091225_fix_bugsv2")]
    partial class fix_bugsv2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API_MortalKombat.Models.Arma", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Armas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Descripcion = "Espada de hielo",
                            FechaActualizacion = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaCreacion = new DateTime(2024, 1, 10, 6, 12, 25, 539, DateTimeKind.Local).AddTicks(8727),
                            Nombre = "Kori Blade"
                        });
                });

            modelBuilder.Entity("API_MortalKombat.Models.Clan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clanes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Descripcion = "Lin Kuei, el clan de fuertes guerreros Ninja chinos, es una sociedad ubicada al norte de Asia, que mata por dinero desde muchas generaciones atrás. Sus integrantes son instruídos desde muy temprana edad, y apenas tienen contacto con el mundo exterior. Los mejores guerreros del clan tienen algún poder especial, y cuando llegan a dominarlo, reciben un nombre clave. Se descubrió que solo los que vienen de un linaje antiguo, llegan a dominar este poder.",
                            FechaActualizacion = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaCreacion = new DateTime(2024, 1, 10, 6, 12, 25, 539, DateTimeKind.Local).AddTicks(8623),
                            Nombre = "Lin Kuei"
                        },
                        new
                        {
                            Id = 2,
                            Descripcion = "El clan Shirai Ryu se formó hace muchos años por obra de un guerrero Lin Kuei llamado Takeda, quien abandonó su clan. Al abandonar el Lin Kuei,Takeda se convirtió en un ninja desertor,crimen que se castiga con la muerte.Takeda fue buscado por los asesinos Lin Kuei.Escapó de China y regresó a Japón,su patria,donde ofreció servicios a los señores y generales.Su técnica gradualmente se extendió por todo Japón, y se convirtió en el arte del Ninjutsu.Además de enseñar su nueva forma de arte marcial,también enseñaba versiones modificadas de las tácticas Lin Kuei,de modo que reveló muchos de sus secretos.Esto avivó la furia de los Lin Kuei.Las enseñanzas de Takeda se hicieron conocidas en todo el Japón por obra de sus muchos seguidores,los cuales pasaron a ser conocidos como el clan Shirai Ryu.",
                            FechaActualizacion = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaCreacion = new DateTime(2024, 1, 10, 6, 12, 25, 539, DateTimeKind.Local).AddTicks(8667),
                            Nombre = "Shirai Ryu"
                        });
                });

            modelBuilder.Entity("API_MortalKombat.Models.EstiloDePelea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EstilosDePeleas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Descripcion = "Arte Marcial",
                            FechaActualizacion = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaCreacion = new DateTime(2024, 1, 10, 6, 12, 25, 539, DateTimeKind.Local).AddTicks(8742),
                            Nombre = "Kung Fu"
                        });
                });

            modelBuilder.Entity("API_MortalKombat.Models.Personaje", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Alineacion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClanId")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagenURl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Raza")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReinoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClanId");

                    b.HasIndex("ReinoId");

                    b.ToTable("Personajes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Alineacion = "Bien",
                            ClanId = 1,
                            Descripcion = "Hermano menor del Sub-Zero original, comenzó reemplazando a su hermano caído y terminó convirtiéndose en el nuevo Gran Maestro Lin Kuei. Como descendiente de los cryomancers, conserva las habilidades para congelar.",
                            FechaActualizacion = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaCreacion = new DateTime(2024, 1, 10, 6, 12, 25, 539, DateTimeKind.Local).AddTicks(8708),
                            ImagenURl = "",
                            Nombre = "Sub-Zero",
                            Raza = "Humano/Cryomancer",
                            ReinoId = 1
                        });
                });

            modelBuilder.Entity("API_MortalKombat.Models.Reino", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Reinos");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Descripcion = "La tierra es uno de los muchos reinos que crearon los antiguos dioses, dicho reino ha sido objeto frecuente de intentos de conquistas, porque se piensa que es la joya del cosmos y un importante centro de energía universal. Debido a esto, los dioses han designado una deidad para proteger el reino de daños, por innumerables siglos éste ha sido Raiden, aunque también otros dioses como Fujin, desempeñan un papel en esta labor.",
                            FechaActualizacion = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FechaCreacion = new DateTime(2024, 1, 10, 6, 12, 25, 539, DateTimeKind.Local).AddTicks(8686),
                            Nombre = "Tierra"
                        });
                });

            modelBuilder.Entity("ArmaPersonaje", b =>
                {
                    b.Property<int>("ArmasId")
                        .HasColumnType("int");

                    b.Property<int>("PersonajesId")
                        .HasColumnType("int");

                    b.HasKey("ArmasId", "PersonajesId");

                    b.HasIndex("PersonajesId");

                    b.ToTable("ArmaPersonaje");
                });

            modelBuilder.Entity("EstiloDePeleaPersonaje", b =>
                {
                    b.Property<int>("EstilosDePeleasId")
                        .HasColumnType("int");

                    b.Property<int>("PersonajesId")
                        .HasColumnType("int");

                    b.HasKey("EstilosDePeleasId", "PersonajesId");

                    b.HasIndex("PersonajesId");

                    b.ToTable("EstiloDePeleaPersonaje");
                });

            modelBuilder.Entity("API_MortalKombat.Models.Personaje", b =>
                {
                    b.HasOne("API_MortalKombat.Models.Clan", "Clan")
                        .WithMany()
                        .HasForeignKey("ClanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API_MortalKombat.Models.Reino", "Reino")
                        .WithMany()
                        .HasForeignKey("ReinoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clan");

                    b.Navigation("Reino");
                });

            modelBuilder.Entity("ArmaPersonaje", b =>
                {
                    b.HasOne("API_MortalKombat.Models.Arma", null)
                        .WithMany()
                        .HasForeignKey("ArmasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API_MortalKombat.Models.Personaje", null)
                        .WithMany()
                        .HasForeignKey("PersonajesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EstiloDePeleaPersonaje", b =>
                {
                    b.HasOne("API_MortalKombat.Models.EstiloDePelea", null)
                        .WithMany()
                        .HasForeignKey("EstilosDePeleasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API_MortalKombat.Models.Personaje", null)
                        .WithMany()
                        .HasForeignKey("PersonajesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}