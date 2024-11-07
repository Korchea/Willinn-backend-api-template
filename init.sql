-- Crear el login
CREATE LOGIN myuser WITH PASSWORD = 'MyPass123';
GO

-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'UserDb')
BEGIN
    CREATE DATABASE UserDb;
END
GO

USE UserDb;
GO

-- Crear el usuario en la base de datos
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'myuser')
BEGIN
    CREATE USER myuser FOR LOGIN myuser;
    ALTER ROLE db_owner ADD MEMBER myuser;
END