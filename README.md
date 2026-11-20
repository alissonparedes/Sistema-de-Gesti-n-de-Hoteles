# Sistema de Gestión de Hoteles (ASP.NET Core 8 MVC + SQL Server)

## Ejecutar
1. Abre SQL Server y ejecuta `Database/script.sql` (crea `HotelDB`, tablas y datos de ejemplo).
2. Ajusta la cadena de conexión `HotelDb` en `appsettings.json` si tu servidor no es `.`
3. Abre `HotelApp.csproj` en Visual Studio 2022 (SDK .NET 8) y presiona F5,
   o en terminal: `dotnet restore && dotnet run`

## Módulos
- Habitaciones: CRUD, filtro por tipo y disponibilidad
- Huéspedes: CRUD, búsqueda por nombre/email, historial de reservas
- Reservas: crear (valida fechas, disponibilidad y solapamiento), calcular total, finalizar, cancelar
