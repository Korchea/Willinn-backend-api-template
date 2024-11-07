using Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class ServiceExtensions
{
    // Configuración de CORS para permitir solo el origen especificado
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder.WithOrigins("http://localhost:3000") // Especifica solo el origen necesario
                    .AllowAnyMethod() // Permite todos los métodos HTTP (GET, POST, etc.)
                    .AllowAnyHeader()); // Permite todos los encabezados
        });
    }

    // Configuración de Swagger para la generación de documentación de la API
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer(); // Expone los endpoints para la documentación
        services.AddSwaggerGen(); // Genera la documentación de Swagger
    }

    // Configuración del DbContext para usar SQL Server
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Data"))); // Especifica el ensamblado de migración como "Data"
    }
}
