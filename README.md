# Proyecto Backend .NET y SQL Server

Este proyecto es una aplicación backend desarrollada con .NET, que utiliza SQL Server como base de datos. La configuración se maneja con Docker y Docker Compose para facilitar la implementación y gestión de la aplicación y la base de datos en contenedores.

## Estructura del Proyecto

El proyecto se organiza de la siguiente manera:

```plaintext
. ├── Api/ # Código fuente de la aplicación backend
  │    ├── Controllers/ # Controladores de la API
  │    ├── Extensions/ # Extensiones del programa
  │    ├── Properties/ # Propiedades del programa
  │    ├── Api.csproj # Archivo del proyecto .NET
  │    ├── appsettings.json # Archivo de configuracion principal
  │    ├── Program.cs # Configuracion del programa
  │    └── Dockerfile # Archivo Dockerfile para construir la imagen del backend
  ├── Core/ # Nucleo central del proyecto
  │    ├── Models/ # Modelos de datos
  │    │     └── Users.cs # Tabla principal
  │    └── Core.csproj # Archivo del proyecto .NET
  ├── Data/ # Configuración de la base de datos y DbContext
  │    ├── Migrations/
  │    ├── AppDbContext.cs # Contexto de DataBase
  │    └── Data.csproj # Archivo del proyecto .NET
  ├── Services/
  ├── UnitTest/
  ├── docker-compose.yml # Configuración de los servicios Docker
  └── README.md # Archivo de documentación
```

## Tecnologías Utilizadas

- .NET 8
- SQL Server 2022 (imagen oficial de Microsoft)
- Docker y Docker Compose

## Configuración

1. Clona el repositorio:
    ```bash
    git clone https://github.com/Korchea/Willinn-backend-api-template.git
    ```
2. Navega al directorio del proyecto:
    ```bash
    cd Willinn-backend-api-template
    ```
3. Configura la conexión con tu servidor de SQL Server en `Api/appsettings.json`:
    ```
    "ConnectionStrings": {
       "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=UserDb;Trusted_Connection=True;TrustServerCertificate=True;"
    }
    ```

## Configuración de Variables de Entorno

Las credenciales y el nombre de la base de datos en `docker-compose.yml` pueden configurarse de la siguiente manera:

```yaml
services:
  sqlserver:
    environment:
      - ACCEPT_EULA=Y
      - MSSQL_SA_PASSWORD=YourStrong@Passw0rd
      - MSSQL_DATABASE=UserDb


  backend:
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=UserDb;User Id=sa;Password=YourStrong@Passw0rd;Encrypt=False;
```

## Ejecutar la Aplicación en Desarrollo

Para iniciar la aplicación:

```bash
dotnet run --project Api/Api.csproj
```

Esto ejecutará la aplicación en `http://localhost:5000` de forma predeterminada. Puedes acceder a ella desde tu navegador.

## API REST

La aplicación crea una API REST que permite:

- **POST /api/Users/login**: Iniciar sesión.
- **POST /api/Users**: Crear un nuevo usuario.
- **GET /api/Users/{id}**: Obtener la información de un usuario por ID.
- **PUT /api/Users/{id}**: Editar la información de un usuario.
- **DELETE /api/Users/{id}**: Eliminar un usuario.

## Autor

Template: Rodrigo Mato - [GitHub](https://github.com/RodrigoMato00)
App: Guillermo Vega - [GitHub](https://github.com/Korchea)