using Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Ejemplo de DbSet genérico
        // public DbSet<YourEntity> YourEntities { get; set; }

        public DbSet<Entities.Usuario> Usuarios { get; set; }
        public DbSet<Estudiante> Estudiante { get; set; }
        public DbSet<Curso> Curso { get; set; }
        public DbSet<Inscripcion> Inscripcion { get; set; }
        public DbSet<SemesterEnrollment> SemesterEnrollments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Cadena de conexión temporal (rellena después)
                optionsBuilder.UseMySQL("server=;database=;user=;password=;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Mapear entidades a tablas con nombres exactos
            modelBuilder.Entity<Estudiante>().ToTable("Estudiante");
            modelBuilder.Entity<Curso>().ToTable("Curso");
            modelBuilder.Entity<Inscripcion>().ToTable("Inscripcion");
        }
    }
} 