# Proyecto Backend con Docker y SQL Server

Este proyecto es una aplicación backend desarrollada con .NET, que utiliza SQL Server como base de datos. La configuración se maneja con Docker y Docker Compose para facilitar la implementación y gestión de la aplicación y la base de datos en contenedores.

## Estructura del Proyecto

El proyecto se organiza de la siguiente manera:

. ├── Api/ # Código fuente de la aplicación backend │ ├── Controllers/ # Controladores de la API
  ├── Models/ # Modelos de datos │ ├── Data/ # Configuración de la base de datos y DbContext 
│ └── Dockerfile # Archivo Dockerfile para construir la imagen del backend ├── docker-compose.yml # Configuración de los servicios Docker └── README.md # Archivo de documentación

## Tecnologías Utilizadas

- .NET 8
- SQL Server 2022 (imagen oficial de Microsoft)
- Docker y Docker Compose

## Configuración de `docker-compose.yml`

El archivo `docker-compose.yml` define dos servicios:

1. **sqlserver**: Servicio que ejecuta SQL Server.
   - Expone el puerto `1433`, el puerto predeterminado de SQL Server.
   - Configura las variables de entorno necesarias (`ACCEPT_EULA`, `MSSQL_SA_PASSWORD` y `MSSQL_DATABASE`) para inicializar SQL Server.
   - Utiliza un volumen llamado `sqlserver_data` para almacenar datos de manera persistente.

2. **backend**: Servicio que ejecuta la aplicación backend.
   - Expone el puerto `5000` para acceder a la aplicación.
   - Configura la conexión con SQL Server mediante una cadena de conexión (`ConnectionStrings__DefaultConnection`) en el bloque `environment`.
   - Depende del servicio `sqlserver` para garantizar que la base de datos esté disponible antes de que la aplicación intente conectarse.

## Requisitos Previos

Asegúrate de tener los siguientes programas instalados en tu sistema:

1. [Docker](https://docs.docker.com/get-docker/)
2. [Docker Compose](https://docs.docker.com/compose/install/)

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
