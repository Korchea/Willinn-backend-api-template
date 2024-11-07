using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    // Clase de contexto de base de datos para interactuar con la base de datos
    public class AppDbContext : DbContext
    {
        // Constructor que pasa las opciones de configuración al constructor base
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet que representa la tabla de usuarios en la base de datos
        public DbSet<User> Users { get; set; }

        // Método para configurar las entidades y su comportamiento
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aquí puedes agregar configuraciones adicionales para las entidades, si es necesario
            // Ejemplo: Configuración de restricciones, relaciones entre tablas, etc.
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id); // Establece la clave primaria para la tabla Users
                entity.Property(u => u.Name).IsRequired(); // Asegura que el campo Name sea obligatorio
                entity.Property(u => u.Email).IsRequired(); // Asegura que el campo Email sea obligatorio
                entity.Property(u => u.Password).IsRequired(); // Asegura que el campo Password sea obligatorio
            });
        }
    }
}
