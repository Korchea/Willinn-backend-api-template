USE [master];
GO

-- Verificar si la base de datos ya existe antes de crearla
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'UserDb')
BEGIN
    CREATE DATABASE UserDb;
END
GO

-- Cambiar a la base de datos UserDb
USE UserDb;
GO

-- Crear la tabla Users
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(MAX) NOT NULL,
    Password NVARCHAR(MAX) NOT NULL,
    IsActive BIT NOT NULL
);
GO

-- Verificar si el login ya existe antes de crearlo
USE [master];  -- Cambiar al contexto de master para crear el login
GO
IF NOT EXISTS (SELECT * FROM sys.sql_logins WHERE name = 'newuser')
BEGIN
    CREATE LOGIN [newuser] WITH PASSWORD = 'Sa_password123!', CHECK_POLICY = OFF;
    ALTER SERVER ROLE [sysadmin] ADD MEMBER [newuser];
END
GO