# API Documentation (Prueba.WebApi)

Base URL (local):
- `http://localhost:5000`

Swagger:
- `http://localhost:5000/swagger`

---

## Convenciones
- Requests/Responses en JSON
- Los errores se devuelven como:
  - `400` cuando hay `ArgumentException` / `ArgumentOutOfRangeException` (negocio/entrada inválida)
  - `500` cuando ocurre un error inesperado

Ejemplo error:
```json
{
  "error": "Unexpected error",
  "detail": "..."
}
