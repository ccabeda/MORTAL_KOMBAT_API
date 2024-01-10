using Microsoft.EntityFrameworkCore;
using MortalKombat_API.Models;

namespace MortalKombat_API.Data
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Personaje> Personajes { get; set; }
        public DbSet<Clan> Clanes { get; set; }
        public DbSet<Reino> Reinos { get; set; }
        public DbSet<Arma> Armas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Arma>()
               .HasMany(e => e.Personajes)
               .WithMany(e => e.Armas);

            modelBuilder.Entity<Clan>().HasData(
                new Clan
                {
                    Id = 1,
                    Nombre = "Lin Kuei",
                    Descripcion = "Lin Kuei, el clan de fuertes guerreros Ninja chinos, es una sociedad ubicada al norte de Asia, que mata por dinero " +
                "desde muchas generaciones atrás. Sus integrantes son instruídos desde muy temprana edad, y apenas tienen contacto con el mundo exterior. Los mejores guerreros del " +
                "clan tienen algún poder especial, y cuando llegan a dominarlo, reciben un nombre clave. Se descubrió que solo los que vienen de un linaje antiguo, llegan a dominar este poder.",
                    FechaCreacion = DateTime.Now
                }
            );

            modelBuilder.Entity<Clan>().HasData(
               new Clan
               {
                   Id = 2,
                   Nombre = "Shirai Ryu",
                   Descripcion = "El clan Shirai Ryu se formó hace muchos años por obra de un guerrero Lin Kuei llamado Takeda, quien abandonó su clan. Al abandonar el Lin Kuei," +
                   "Takeda se convirtió en un ninja desertor,crimen que se castiga con la muerte.Takeda fue buscado por los asesinos Lin Kuei.Escapó de China y regresó a Japón,su " +
                   "patria,donde ofreció servicios a los señores y generales.Su técnica gradualmente se extendió por todo Japón, y se convirtió en el arte del Ninjutsu.Además de " +
                   "enseñar su nueva forma de arte marcial,también enseñaba versiones modificadas de las tácticas Lin Kuei,de modo que reveló muchos de sus secretos.Esto avivó la furia de los Lin Kuei." +
                   "Las enseñanzas de Takeda se hicieron conocidas en todo el Japón por obra de sus muchos seguidores,los cuales pasaron a ser conocidos como el clan Shirai Ryu.",
                   FechaCreacion = DateTime.Now
               }
           );

            modelBuilder.Entity<Reino>().HasData(
                new Reino
                {
                    Id = 1,
                    Nombre = "Tierra",
                    Descripcion = "La tierra es uno de los muchos reinos que crearon los antiguos dioses, dicho reino ha sido objeto frecuente de " +
                "intentos de conquistas, porque se piensa que es la joya del cosmos y un importante centro de energía universal. Debido a esto, los dioses han designado una " +
                "deidad para proteger el reino de daños, por innumerables siglos éste ha sido Raiden, aunque también otros dioses como Fujin, desempeñan un papel en esta labor.",
                    FechaCreacion = DateTime.Now
                }

            );

            modelBuilder.Entity<Personaje>().HasData(
                new Personaje
                {
                    Id = 1,
                    Nombre = "Sub-Zero",
                    ImagenURl = "",
                    Alineacion = "Bien",
                    Raza = "Humano/Cryomancer",
                    Descripcion = "Hermano menor del Sub-Zero original, comenzó reemplazando a su hermano caído y terminó convirtiéndose en el nuevo Gran Maestro Lin Kuei. " +
                    "Como descendiente de los cryomancers, conserva las habilidades para congelar.",
                    ClanId = 1,
                    ReinoId = 1,
                    EstilosDePelea = new List<string> {"Shotokan","Dragón"},
                    FechaCreacion = DateTime.Now
                }
            );

            modelBuilder.Entity<Arma>().HasData(
                new Arma
                {
                    Id = 1,
                    Nombre = "Kori Blade",
                    Descripcion = "Espada de hielo",
                    FechaCreacion = DateTime.Now
                }
            );

            modelBuilder.Entity<Arma>().HasData(
                new Arma
                {
                    Id = 2,
                    Nombre = "Kunai",
                    Descripcion = "Arma afilada",
                    FechaCreacion = DateTime.Now
                }
            );

        }
    }
}