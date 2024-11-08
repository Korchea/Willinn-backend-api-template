# Prueba Tecnica Willinn Backend .NET y SQL Server

Este proyecto es una aplicación backend desarrollada con .NET, que implementa un sistema de CRUD de usuarios con autenticación de inicio de sesión. Utiliza SQL Server como base de datos, y Docker y Docker Compose para facilitar la implementación y gestión de la aplicación en contenedores.

## Estructura del Proyecto

El proyecto está organizado de la siguiente manera:

```plaintext
.
├── Api/                         # Código fuente de la aplicación backend
│   ├── Controllers/             # Controladores de la API
│   ├── Extensions/              # Extensiones del programa
│   ├── Properties/              # Propiedades del programa
│   ├── Api.csproj               # Archivo de proyecto .NET
│   ├── appsettings.json         # Archivo principal de configuración
│   ├── Program.cs               # Configuración principal del programa
│   └── Dockerfile               # Archivo Dockerfile para construir la imagen del backend
├── Core/                        # Núcleo del proyecto
│   ├── Models/                  # Modelos de datos
│   │   └── Users.cs             # Modelo de la tabla principal (usuarios)
│   └── Core.csproj              # Archivo de proyecto .NET
├── Data/                        # Configuración de base de datos y DbContext
│   ├── Migrations/              # Migraciones de la base de datos
│   ├── AppDbContext.cs          # Contexto de base de datos
│   └── Data.csproj              # Archivo de proyecto .NET
├── Services/                    # Servicios adicionales de la aplicación
├── UnitTest/                    # Pruebas unitarias
├── docker-compose.yml           # Configuración de servicios Docker
└── README.md                    # Archivo de documentación
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


  backend:
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=UserDb;User Id=sa;Password=YourStrong@Passw0rd;Encrypt=False;
```

## Ejecutar la Aplicación

Para iniciar la aplicación de forma local:

```bash
dotnet run --project Api/Api.csproj
```

Esto ejecutará la aplicación en `http://localhost:5000` de forma predeterminada. Asegúrate de que tu SQL Server está en ejecución y que tienes la base de datos y la tabla configuradas, o ejecuta una migración de datos usando los archivos en `./Data/Migrations` si es necesario.

### Para hacer la migracion tienes que 

1. Abre la terminal en el directorio raíz del proyecto (donde está ubicado el archivo `.csproj` de la capa de datos).

2. Asegúrate de que la cadena de conexión en `appsettings.json` o en el archivo de entorno de Docker (`docker-compose.yml`) esté configurada correctamente para apuntar al servidor de SQL Server.

3. Ejecuta los siguientes comandos en la terminal:

#### Paso 1: Agregar la migración inicial

```bash
dotnet ef migrations add InitialMigration --project Data --startup-project Api
```
Esto creará los archivos de migración en la carpeta Migrations dentro de la capa de datos (`Data`).

#### Paso 2: Aplicar la migración a la base de datos

```bash
dotnet ef database update --project Data --startup-project Api
```

Este comando aplicará la migración a la base de datos configurada, creando las tablas y relaciones definidas en el modelo (`AppDbContext` y otros modelos de entidad).

Nota: Asegúrate de tener `dotnet-ef` instalado. Si no lo tienes, puedes instalarlo ejecutando:

```bash
dotnet tool install --global dotnet-ef
```

### Para iniciar la aplicacion en Docker:

```bash
docker-compose up --build
```

Este comando crea la imagen en Docker y ejecuta la aplicación. La aplicación estará accesible en `http://localhost:8080`. Asegúrate de que la cadena de conexión en `appsettings.json` esté correctamente configurada para esta ejecución.

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