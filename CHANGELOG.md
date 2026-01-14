# Changelog

Todos los cambios notables del proyecto se documentan aquí.

El formato está inspirado en Keep a Changelog y SemVer, pero adaptado a una prueba técnica.

---

## [Unreleased]
- Mejoras potenciales:
  - Normalización de entidades `Customer` y `Product` (tablas + FK).
  - Más pruebas unitarias por capa y cobertura en CI.
  - Observabilidad (logging estructurado, correlation IDs).
  - Validaciones de negocio extendidas por producto/cliente.

---

## [1.0.0] - 2026-01-13
### Added
- Arquitectura por capas:
  - `Domain`: Entidades, Value Objects, reglas y servicios de dominio.
  - `Application`: CQRS (Commands/Queries), Handlers (MediatR), DTOs, reportes.
  - `Infrastructure`: EF Core DbContext, repositorios, migraciones.
  - `WebApi`: Endpoints API + Swagger.
  - `Web`: MVC con Bootstrap para interacción básica.
- CQRS + MediatR:
  - Command: `CreateOrderCommand` + `CreateOrderHandler`
  - Query: `GetOrdersByCustomerQuery` + `GetOrdersByCustomerHandler`
  - Report Query: `CustomerIntervalsReportExcelQuery` + `CustomerIntervalsReportExcelHandler`
- EF Core:
  - `AppDbContext` con tabla `Orders`
  - Migración inicial y `database update` funcional
- Reporte Excel:
  - Exportación con ClosedXML: `/api/Reports/customer-intervals/excel`
- Validaciones:
  - FluentValidation (CreateOrderCommandValidator)
  - Reglas de dominio `DistanceRules`
- Manejo global de errores:
  - Middleware/ExceptionHandler que convierte excepciones en respuestas JSON (400/500)

### Fixed
- Configuración DI estable para ejecución y tooling de EF Core.
- Ajustes de endpoints para responder correctamente en API y permitir consumo desde Web.
- Compatibilidad con herramientas EF Core (actualización de versión cuando aplica).

---
