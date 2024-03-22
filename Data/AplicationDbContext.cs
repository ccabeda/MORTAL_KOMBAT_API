using API_MortalKombat.Models;
using Microsoft.EntityFrameworkCore;

namespace API_MortalKombat.Data
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options) 
        {}

        public DbSet<Personaje> Personajes { get; set; }
        public DbSet<Clan> Clanes { get; set; }
        public DbSet<Reino> Reinos { get; set; }
        public DbSet<Arma> Armas { get; set; }
        public DbSet<EstiloDePelea> EstilosDePeleas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Arma>() //para hacer una relacion muchos a muchos utilizo esto 
               .HasMany(e => e.Personajes)
               .WithMany(e => e.Armas);

            modelBuilder.Entity<EstiloDePelea>()
               .HasMany(e => e.Personajes)
               .WithMany(e => e.EstilosDePeleas);

            modelBuilder.Entity<Clan>().HasData( //creo prefedinidos objetos en la db con migraciones
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
            modelBuilder.Entity<EstiloDePelea>().HasData(
                new EstiloDePelea
                {
                    Id = 1,
                    Nombre = "Kung Fu",
                    Descripcion = "Arte Marcial",
                    FechaCreacion = DateTime.Now
                }
            );
            modelBuilder.Entity<Rol>().HasData(
                new Rol
                {
                    Id = 1,
                    Nombre = "Super Administrador",
                }
            );
            modelBuilder.Entity<Rol>().HasData(
                new Rol
                {
                    Id = 2,
                    Nombre = "Administrador",
                }
            );
            modelBuilder.Entity<Rol>().HasData(
                new Rol
                {
                    Id = 3,
                    Nombre = "Publico",
                }
            );
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    NombreDeUsuario = "pperez_",
                    Nombre = "Pablo",
                    Apellido = "Perez",
                    Mail = "pperez@gmail.com",
                    Contraseña = "123.@",
                    RolId = 1,
                    FechaCreacion = DateTime.Now
                }
            );
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 2,
                    NombreDeUsuario = "lauti.cai",
                    Nombre = "Lautaro",
                    Apellido = "Mas",
                    Mail = "lautimas@gmail.com",
                    Contraseña = "123.@",
                    RolId = 2,
                    FechaCreacion = DateTime.Now
                }
            );
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 3,
                    NombreDeUsuario = "pulga",
                    Nombre = "Sergio",
                    Apellido = "Gonzalez",
                    Mail = "sergi02002@gmail.com",
                    Contraseña = "123.@",
                    RolId = 3,
                    FechaCreacion = DateTime.Now
                }
            );
        }
    }
}