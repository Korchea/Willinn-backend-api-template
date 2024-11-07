using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Agrega servicios al contenedor
builder.Services.ConfigureDbContext(builder.Configuration); // Configuración del DbContext para SQL Server
builder.Services.AddControllers(); // Agrega los controladores para manejar las solicitudes
builder.Services.ConfigureSwagger(); // Configuración de Swagger para la documentación de la API
builder.Services.ConfigureCors(); // Configuración de CORS para permitir solicitudes desde orígenes específicos

// Crea la aplicación
var app = builder.Build();

// Configura CORS para que utilice la política "CorsPolicy"
app.UseCors("CorsPolicy");

// Configura el pipeline de manejo de solicitudes HTTP
// Swagger solo para entornos de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirección HTTPS (descomentarlo si deseas redirigir todas las solicitudes a HTTPS)
// app.UseHttpsRedirection();

app.UseAuthorization(); // Configura la autorización

app.MapControllers(); // Mapea los controladores para habilitar los endpoints

app.Run(); // Ejecuta la aplicación
