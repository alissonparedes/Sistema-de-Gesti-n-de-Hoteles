IF DB_ID('HotelDB') IS NULL CREATE DATABASE HotelDB;
GO
USE HotelDB;
GO
CREATE TABLE Habitaciones (
    habitacion_id INT IDENTITY(1,1) PRIMARY KEY,
    numero        VARCHAR(10)   NOT NULL UNIQUE,
    tipo          VARCHAR(30)   NOT NULL,
    precio        DECIMAL(10,2) NOT NULL CHECK (precio > 0),
    disponible    BIT           NOT NULL DEFAULT 1
);
CREATE TABLE Huespedes (
    huesped_id INT IDENTITY(1,1) PRIMARY KEY,
    nombre     VARCHAR(60)  NOT NULL,
    apellido   VARCHAR(60)  NOT NULL,
    email      VARCHAR(120) NOT NULL UNIQUE,
    telefono   VARCHAR(20)
);
CREATE TABLE Reservas (
    reserva_id    INT IDENTITY(1,1) PRIMARY KEY,
    habitacion_id INT NOT NULL REFERENCES Habitaciones(habitacion_id),
    huesped_id    INT NOT NULL REFERENCES Huespedes(huesped_id),
    fecha_entrada DATE NOT NULL,
    fecha_salida  DATE NOT NULL,
    estado        VARCHAR(15) NOT NULL DEFAULT 'Activa',
    total         DECIMAL(10,2) NOT NULL,
    CONSTRAINT CK_Reservas_Fechas CHECK (fecha_salida > fecha_entrada)
);
GO
INSERT INTO Habitaciones (numero, tipo, precio, disponible) VALUES
('101','Simple',40,1),('102','Simple',40,1),('201','Doble',65,1),('202','Doble',65,1),('301','Suite',120,1);
INSERT INTO Huespedes (nombre, apellido, email, telefono) VALUES
('Ana','Torres','ana.torres@mail.com','0991234567'),
('Luis','Mena','luis.mena@mail.com','0987654321');
GO
