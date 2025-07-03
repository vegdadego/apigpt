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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Cadena de conexión temporal (rellena después)
                optionsBuilder.UseMySQL("server=;database=;user=;password=;");
            }
        }
    }
} 