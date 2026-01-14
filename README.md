# Prueba FullStack (.NET) — CQRS + MediatR + EF Core + Web + API + Excel

Solución FullStack con:
- **API**: ASP.NET Core Web API (Swagger)
- **Web**: ASP.NET Core MVC (Bootstrap)
- **Arquitectura**: CQRS + MediatR (Commands/Queries + Handlers)
- **Persistencia**: Entity Framework Core + SQL Server (Docker)
- **Validaciones**: FluentValidation
- **Reporte**: Exportación **Excel** (ClosedXML)

---

## Requisitos
- .NET SDK (el del `global.json` del proyecto)
- Docker Desktop
- (Opcional) SQL Server Management Studio o Azure Data Studio

---

## Estructura del proyecto

- `Prueba.Domain`
  - Entidades, ValueObjects, Reglas de negocio y Servicios de dominio (Haversine, rules, etc.)
- `Prueba.Application`
  - CQRS: Commands/Queries + Handlers, DTOs, Validadores, Reportes (Excel)
- `Prueba.Infrastructure`
  - EF Core DbContext, Migrations, Repositories
- `Prueba.WebApi`
  - Endpoints API + Swagger
- `Prueba.Web`
  - Interfaz Web MVC (Bootstrap)
- `Prueba.Tests`
  - Pruebas unitarias/smoke

---

## Configuración de base de datos (SQL Server con Docker)

> Si el puerto 1433 está ocupado, revisa qué proceso/servicio lo usa antes de crear el contenedor.

### 1) Verificar contenedores
```powershell
docker ps -a
