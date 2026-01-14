# Prueba FullStack (.NET) — CQRS + MediatR + EF Core + Web + API + Excel

Solución **FullStack** con arquitectura limpia por capas y patrones **CQRS + MediatR**, con **API (Swagger)**, **Web MVC (Bootstrap)**, persistencia con **EF Core + SQL Server (Docker)** y reporte **Excel (ClosedXML)**.

---

## ✅ Stack / Características

- **API:** ASP.NET Core Web API + **Swagger**
- **Web:** ASP.NET Core MVC + **Bootstrap**
- **Arquitectura:** **CQRS + MediatR** (Commands/Queries + Handlers)
- **Persistencia:** **Entity Framework Core** + SQL Server
- **Infra DB:** SQL Server en **Docker**
- **Validaciones:** **FluentValidation**
- **Reporte:** Exportación **Excel** con **ClosedXML**
- **Tests:** Unit tests / smoke tests (proyecto `Prueba.Tests`)

---

## ✅ Requisitos

- **.NET SDK**: el que indica el `global.json` del repo  
- **Docker Desktop**
- (Opcional) **SSMS** o **Azure Data Studio** para inspeccionar la DB

---

## 📦 Estructura del proyecto

Prueba.Domain

Entidades, ValueObjects, reglas de negocio, servicios de dominio
Prueba.Application

CQRS: Commands/Queries + Handlers, DTOs, validadores, reportes (Excel)
Prueba.Infrastructure

EF Core DbContext, Migrations, Repositories
Prueba.WebApi

Endpoints API + Swagger
Prueba.Web

Interfaz Web MVC (Bootstrap)
Prueba.Tests

Pruebas unitarias / smoke tests

yaml
Copiar código

---

## ⚙️ Configuración de base de datos (SQL Server con Docker)

> El proyecto usa una connection string con:
> `Server=localhost,1433;Database=PruebaFullStackDb;User Id=sa;Password=Passw0rd!123;TrustServerCertificate=True;`

### 0) Si el puerto 1433 está ocupado

**Ver qué lo está usando:**

**PowerShell**
```powershell
netstat -ano | findstr :1433
Con el PID que salga:

powershell
Copiar código
tasklist /FI "PID eq <PID>"
Si resulta que hay un SQL Server local usando el puerto, puedes:

detener el servicio local, o

levantar el contenedor en otro puerto (ej. 14330:1433) y ajustar la connection string.

1) Verificar contenedores
bash
Copiar código
docker ps -a
Busca si ya existe uno llamado sql-prueba.

2) Crear SQL Server en Docker (recomendado)
Opción A: Puerto estándar 1433 (si está libre)
bash
Copiar código
docker run -d --name sql-prueba ^
  -e "ACCEPT_EULA=Y" ^
  -e "MSSQL_SA_PASSWORD=Passw0rd!123" ^
  -p 1433:1433 ^
  mcr.microsoft.com/mssql/server:2022-latest
Opción B: Puerto alterno si 1433 está ocupado
bash
Copiar código
docker run -d --name sql-prueba ^
  -e "ACCEPT_EULA=Y" ^
  -e "MSSQL_SA_PASSWORD=Passw0rd!123" ^
  -p 14330:1433 ^
  mcr.microsoft.com/mssql/server:2022-latest
Y en ese caso cambia el appsettings.json del WebApi a:

json
Copiar código
"Default": "Server=localhost,14330;Database=PruebaFullStackDb;User Id=sa;Password=Passw0rd!123;TrustServerCertificate=True;"
3) Ver logs del SQL (por si falla)
bash
Copiar código
docker logs -f sql-prueba
4) Ejecutar migraciones EF Core (crear DB y tablas)
Desde la raíz del repo:

powershell
Copiar código
cd C:\pruebatecnicanet\PruebaFullStack
Ejecutar migración:

powershell
Copiar código
dotnet ef database update `
  --project .\Prueba.Infrastructure\Prueba.Infrastructure.csproj `
  --startup-project .\Prueba.WebApi\Prueba.WebApi.csproj
Nota: Se incluyó AppDbContextFactory para permitir que EF Core encuentre la connection string en design-time.

▶️ Correr la solución
1) Build general
powershell
Copiar código
dotnet build
🚀 Ejecutar API (Swagger)
powershell
Copiar código
dotnet run --project .\Prueba.WebApi\Prueba.WebApi.csproj
URLs típicas:

Swagger: http://localhost:5000/swagger

API base: http://localhost:5000/api

🌐 Ejecutar Web MVC (Bootstrap)
En otra terminal:

powershell
Copiar código
dotnet run --project .\Prueba.Web\Prueba.Web.csproj
URL típica:

Web: http://localhost:5100/

Si tu launchSettings.json define otros puertos, usa los que te muestre la consola al ejecutar.

🧪 Probar endpoints API (PowerShell)
En PowerShell NO uses curl -L porque curl es alias de Invoke-WebRequest.
Usa Invoke-RestMethod o Invoke-WebRequest.

1) Health (si existe)
powershell
Copiar código
Invoke-RestMethod "http://localhost:5000/health"
2) Crear Orden (POST)
powershell
Copiar código
$body = @{
  customer = "prueba"
  product = "prueba"
  quantity = 2
  originLat = 4.7110
  originLon = -74.0721
  destinationLat = 4.7300
  destinationLon = -74.0500
} | ConvertTo-Json

Invoke-RestMethod `
  -Method Post `
  -Uri "http://localhost:5000/api/Orders" `
  -ContentType "application/json" `
  -Body $body
3) Consultar órdenes por customer (GET)
powershell
Copiar código
Invoke-RestMethod "http://localhost:5000/api/Orders?customer=prueba"
4) Descargar reporte Excel
powershell
Copiar código
Invoke-WebRequest `
  "http://localhost:5000/api/Reports/customer-intervals/excel" `
  -OutFile ".\customer-intervals.xlsx"

Get-Item .\customer-intervals.xlsx | Select Name, Length
📊 Reporte Excel (Customer Intervals)
El reporte genera columnas:

Customer

1-50

51-200

201-500

501-1000

Total

Se construye desde Prueba.Application.Reports.CustomerIntervals con:

CustomerIntervalsReportService (genera DTO/resultado)

CustomerIntervalsExcelExporter (ClosedXML)

✅ CQRS + MediatR (Cómo está aplicado)
Commands: mutan estado (ej. CreateOrderCommand)

Queries: solo lectura (ej. GetOrdersByCustomerQuery)

Handlers: reciben command/query y ejecutan caso de uso.

Los controllers en API actúan como thin controllers:

construyen command/query

envían por MediatR

retornan respuesta

✅ Validaciones (FluentValidation)
Se usa FluentValidation.AspNetCore.

Los validators viven en Application, ej:

CreateOrderCommandValidator

Se conectan en DI (Program.cs) y se ejecutan antes del handler.

🧱 Entity Framework Core
AppDbContext vive en Prueba.Infrastructure.Persistence

OrderRepository vive en Prueba.Infrastructure.Repositories

Migraciones en Prueba.Infrastructure\Migrations

🛡️ Middleware / Manejo global de errores
UseExceptionHandler captura errores no controlados y retorna JSON:

400 para ArgumentException / ArgumentOutOfRangeException

500 para errores inesperados

✅ Tests
Correr tests:

powershell
Copiar código
dotnet test
Con cobertura:

powershell
Copiar código
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
🧩 Troubleshooting
Swagger no abre / conexión rechazada
Verifica que el puerto está escuchando:

powershell
Copiar código
netstat -ano | findstr :5000
Revisa consola donde corriste dotnet run (por errores).

Docker: “port is already allocated”
Significa que 1433 ya está en uso. Usa:

puerto alterno 14330:1433, o

detén el proceso que usa 1433.

📌 Notas finales
La solución está lista para:

ejecutar API + Web

crear órdenes

consultar órdenes por customer

exportar Excel desde API

persistir datos en SQL Server Docker con migraciones EF Core

