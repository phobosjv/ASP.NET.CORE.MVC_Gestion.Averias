namespace GestionIncidencias
{
    // Para Entity Framework Core
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TipoDispositivo> TiposDispositivos { get; set; } // Especifica que la tabla TiposDispositivos se creará desde la clase TipoDispositivo

        public DbSet<Tecnico> Tecnicos { get; set;} // Especifica que la tabla Tecnicos se creará desde la clase Tecnico

        public DbSet<Oficina> Oficinas { get; set; } // Especifica que la tabla Oficinas se creará desde la clase Oficina

        public DbSet<ModeloDispositivo> ModelosDispositivos { get; set; } // Especifica que la tabla ModelosDispositivos se creará desde la clase ModeloDispositivo

        public DbSet<Dispositivo> Dispositivos { get; set; } // Especifica que la tabla Dispositivos se creará desde la clase Dispositivo.

        public DbSet<Incidencia> Incidencias { get; set;} // Especifica que la tabla Incidencias se creará desde la clase Incidencia.
    }
}
