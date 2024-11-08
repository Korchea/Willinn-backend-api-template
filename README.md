# Prueba Tecnica Willinn Backend .NET y SQL Server

Este proyecto es una aplicación backend desarrollada con .NET, que implementa un sistema de CRUD de usuarios con autenticación de inicio de sesión. Utiliza SQL Server como base de datos, y Docker Compose para facilitar la implementación y gestión de la aplicación en contenedores.

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
├── init/                        # Servicios adicionales de la aplicación
│    └── init.sql                # Crea la base de datos de forma correcta
├── wait-for-it.sh               # Encargado de verificar el correcto funcionamiento de una conexion
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

## Configuración de `docker-compose.yml`

El archivo `docker-compose.yml` define tres servicios:

1. **sqlserver**: Servicio que ejecuta SQL Server.
   - Expone el puerto `1433`, que es el puerto predeterminado de SQL Server, para permitir conexiones externas.
   - Configura las siguientes variables de entorno necesarias para inicializar SQL Server:
     - `ACCEPT_EULA=Y`: Acepta los términos de la licencia de SQL Server.
     - `MSSQL_SA_PASSWORD`: Define la contraseña del usuario `sa`, que es el administrador del sistema en SQL Server.
     - `MSSQL_PID=Developer`: Configura la edición de SQL Server como "Developer", una edición gratuita para desarrollo.
   - Utiliza un volumen llamado `sqlserver_data` para almacenar los datos de la base de datos de manera persistente. Esto asegura que los datos no se pierdan cuando el contenedor se reinicie.
   - El servicio también tiene un **healthcheck** que verifica la disponibilidad de la base de datos ejecutando un simple comando SQL (`SELECT 1`) para garantizar que SQL Server esté listo para aceptar conexiones.

2. **sqlserver.configurator**: Servicio encargado de ejecutar un script de inicialización (`init.sql`) en la base de datos de SQL Server.
   - Utiliza la imagen `mcr.microsoft.com/mssql-tools:latest` que contiene las herramientas necesarias para interactuar con SQL Server.
   - Monta un directorio local `./init` que contiene el archivo `init.sql` que se ejecutará para crear la base de datos y las tablas necesarias.
   - El comando en este servicio asegura que el script `init.sql` se ejecute después de un retraso de 60 segundos, lo que da tiempo suficiente para que SQL Server esté listo.
   - El servicio depende de `sqlserver`, garantizando que SQL Server esté disponible antes de ejecutar el script de inicialización.

3. **backend**: Servicio que ejecuta la aplicación backend.
   - Utiliza un archivo Dockerfile localizado en el directorio `Api/Dockerfile` para construir la imagen del backend.
   - Expone el puerto `8080` para acceder a la API o aplicación backend.
   - Configura la cadena de conexión a la base de datos de SQL Server a través de la variable de entorno `ConnectionStrings__DefaultConnection`, lo que permite que la aplicación se conecte a SQL Server utilizando las credenciales del usuario `sa`.
   - Dependiendo del servicio `sqlserver`, garantiza que la base de datos esté disponible antes de que la aplicación backend intente conectarse.
   - El comando en este servicio ejecuta la actualización de la base de datos mediante `dotnet ef database update` y luego inicia la aplicación backend (`dotnet Api.dll`).

### Redes y Volúmenes

- **Redes**:
  - El archivo define una red de puente llamada `app-network`, que permite la comunicación entre los contenedores de los diferentes servicios (sqlserver, sqlserver.configurator, backend).
  
- **Volúmenes**:
  - Se define un volumen llamado `sqlserver_data` para almacenar los datos de la base de datos de manera persistente. Este volumen asegura que los datos de la base de datos no se pierdan cuando el contenedor de SQL Server se detenga o reinicie.


## Ejecutar la Aplicación

Para iniciar la aplicación de forma local:

```bash
dotnet run --project Api/Api.csproj
```

Esto ejecutará la aplicación en `http://localhost:5000` de forma predeterminada. Asegúrate de que tu SQL Server está en ejecución y que tienes la base de datos y la tabla configuradas, o ejecuta una migración de datos usando los archivos en `./Data/Migrations` si es necesario.

### Para hacer la migracion:

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
### Para iniciar la aplicación en Docker:
```bash
docker-compose up --build
```
Este comando crea las imágenes de Docker y ejecuta la aplicación. Una vez que todos los contenedores se inicialicen correctamente, la aplicación estará accesible en http://localhost:8080/swagger/index.html.

Consideraciones para la correcta inicialización de la base de datos:

En equipos con recursos limitados, puede suceder que el servicio de configuración (sqlserver.configurator) intente ejecutar el archivo init.sql antes de que el servicio de SQL Server (sqlserver) esté completamente listo. Esto podría causar errores de conexión en las consultas iniciales de configuración.

Para asegurarse de que todo funcione como se espera:

Revisar los logs: Observa los logs de sqlserver.configurator para confirmar que se haya ejecutado el script de inicialización sin problemas.

Puedes ejecutar docker-compose logs sqlserver.configurator para ver el estado del proceso de configuración.

Errores comunes: Si encuentras un error de inicio de sesión o conexión, es posible que sqlserver no esté completamente listo cuando sqlserver.configurator intenta conectarse.

En caso de un error de conexión o autenticación, vuelve a ejecutar init.sql manualmente después de confirmar que sqlserver está activo.

Ejecutar el script de configuración manualmente (si es necesario): Si encuentras problemas, sigue estos pasos para reejecutar init.sql:


```bash
docker-compose exec sqlserver.configurator /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P Sa_password123! -d master -i /docker-entrypoint-initdb.d/init.sql
```

Esto ejecutará el script de configuración nuevamente una vez que el servidor SQL esté completamente disponible.

Verificar la conexión: Si todo se ha configurado correctamente, accede a la API a través de http://localhost:8080/swagger/index.html y verifica que la conexión a la base de datos funcione.

Siguiendo estos pasos, puedes resolver rápidamente cualquier problema de conexión inicial y asegurarte de que la aplicación esté lista para usarse.

## API REST

La aplicación crea una API REST que permite:

- **POST /api/Users/login**: Iniciar sesión.
- **POST /api/Users**: Crear un nuevo usuario.
- **GET /api/Users/{id}**: Obtener la información de un usuario por ID.
- **PUT /api/Users/{id}**: Editar la información de un usuario.
- **DELETE /api/Users/{id}**: Eliminar un usuario.

## Autor

- Template: Rodrigo Mato - [GitHub](https://github.com/RodrigoMato00)
- App: Guillermo Vega - [GitHub](https://github.com/Korchea)
