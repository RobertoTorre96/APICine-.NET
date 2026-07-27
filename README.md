# 🎬 ApiCine — Cinema Reservation API

API REST desarrollada con **ASP.NET Core 8** que implementa un sistema completo de reservas de asientos para un cine.

El sistema permite administrar:

- usuarios (con autenticación JWT)
- películas
- géneros
- salas
- funciones
- reservas de asientos

Además implementa control de concurrencia y transacciones para evitar que dos usuarios reserven el mismo asiento para la misma función.

Este proyecto fue desarrollado como práctica de backend con ASP.NET Core, Entity Framework Core y buenas prácticas de arquitectura (feature-based / vertical slices).

## 📌 Tabla de Contenidos
- [Descripción](#-descripción)
- [Datos de prueba](#-datos-de-prueba)
- [Arquitectura](#-arquitectura)
- [Organización del proyecto](#-organización-del-proyecto)
- [Tecnologías](#️-tecnologías)
- [Modelo del dominio](#-modelo-del-dominio)
- [Flujo del sistema](#-flujo-del-sistema)
- [Control de concurrencia](#-control-de-concurrencia)
- [Autenticación y roles](#-autenticación-y-roles)
- [Documentación de API](#-documentación-de-api)
- [Manejo global de errores](#-manejo-global-de-errores)
- [Instalación](#️-instalación)
- [Endpoints principales](#-endpoints-principales)
- [Mejoras futuras](#-mejoras-futuras)
- [Autor / Contacto](#-autor)

## 📖 Descripción
Este proyecto simula el backend de un sistema de reservas de cine.

Permite:

- registrar usuarios y autenticarse mediante JWT
- administrar películas y géneros (relación muchos a muchos)
- crear salas con generación automática de asientos según filas y columnas
- programar funciones, validando que no se solapen horarios en la misma sala
- reservar asientos para una función, validando disponibilidad y capacidad

El sistema está diseñado para evitar problemas de concurrencia en reservas, uno de los problemas más comunes en sistemas de tickets.

## 🔑 Datos de prueba

La base de datos se recrea y se puebla automáticamente en cada arranque (`EnsureDeleted` + `EnsureCreated` + `SeedData.sql`), por lo que ya vas a contar con usuarios, películas, salas y funciones de ejemplo listos para probar.

| Username     | Contraseña   | Rol      | Email              |
|--------------|--------------|----------|---------------------|
| `admin`      | *(hasheada)* | Admin    | admin                |
| `juan_perez` | `12345678`   | Cliente  | juan@gmail.com       |
| `maria_cine` | `12345678`   | Cliente  | maria@gmail.com      |

- El usuario **Admin** tiene acceso a los endpoints de administración (crear películas, salas, funciones, géneros, etc.).
- Los usuarios **Cliente** permiten probar el flujo de reserva de asientos de punta a punta.

> ⚠️ Estas son credenciales de un entorno de **prueba/desarrollo**, no de producción real. Las contraseñas están hasheadas con BCrypt en la base de datos (`Data/Scripts/SeedData.sql`).

## 🏗 Arquitectura
La aplicación sigue una arquitectura en capas:

```
Controller
   ↓
Service
   ↓
DbContext (EF Core)
   ↓
Entity
   ↓
Database (SQLite)
```

Cada capa tiene responsabilidades claras.

| Capa       | Responsabilidad          |
|------------|---------------------------|
| Controller | Exponer endpoints REST    |
| Service    | Lógica de negocio         |
| DbContext  | Acceso a base de datos (EF Core) |
| Entity     | Modelo de dominio         |

## 📦 Organización del Proyecto
El proyecto está organizado por feature (feature-based / vertical slices), en lugar de por capas técnicas globales.

En lugar de tener carpetas globales como `Controllers`, `Services`, `DTOs`, cada entidad del dominio tiene su propio paquete con su Entity, Config (Fluent API), Controller, Service y DTOs.

Esto facilita:

- encontrar rápidamente la lógica relacionada
- mantener el proyecto escalable
- evitar carpetas demasiado grandes
- mejorar la mantenibilidad

Estructura simplificada:

```
ApiCine
│
├── Data
│   ├── AppDbContext.cs
│   └── Scripts/SeedData.sql
│
├── Exceptions
│   ├── GlobalExceptionHandler.cs
│   ├── NotFoundException.cs
│   ├── AlreadyExistsException.cs
│   ├── BadRequestException.cs
│   └── ValidationException.cs
│
├── Features
│   ├── Auth
│   │   ├── Controller / Service / Dto
│   │
│   ├── Usuario
│   │   ├── UsuarioEntity / UsuarioConfig
│   │   ├── DTOs
│   │   └── Service
│   │
│   ├── Pelicula
│   │   ├── PeliculaEntity / PeliculaConfig
│   │   ├── DTOs
│   │   └── Service
│   │
│   ├── Genero
│   │   ├── GeneroEntity / GeneroConfig
│   │   ├── DTOs
│   │   └── Service
│   │
│   ├── Sala
│   │   ├── SalaEntity / SalaConfig
│   │   ├── DTOs
│   │   └── Service
│   │
│   ├── Asiento
│   │   ├── AsientoEntity
│   │   └── DTOs
│   │
│   ├── Funcion
│   │   ├── FuncionEntity / FuncionConfig
│   │   ├── DTOs
│   │   └── Service
│   │
│   ├── Reserva
│   │   ├── ReservaEntity / ReservaConfig
│   │   ├── DTOs
│   │   └── Service
│   │
│   ├── Relaciones
│   │   ├── PeliculaGenero
│   │   └── ReservaAsientos
│   │
│   ├── Enums
│   │   └── EEstadoReserva.cs
│   │
│   └── Role
│       └── ERole.cs
│
└── Mappers
    └── MappingProfile.cs (AutoMapper)
```

## ⚙️ Tecnologías
**Backend**

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8
- SQLite (base de datos embebida)
- Autenticación JWT (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- BCrypt.Net-Next (hash de contraseñas)
- AutoMapper

**Documentación**

- Swagger / OpenAPI (Swashbuckle.AspNetCore)

**Herramientas**

- Migraciones EF Core
- Postman / Swagger UI

## 🧩 Modelo del Dominio
Entidades principales:

- Usuario
- Pelicula
- Genero
- PeliculaGenero (tabla intermedia)
- Sala
- Asiento
- Funcion
- Reserva
- ReservaAsiento (tabla intermedia)

Relación simplificada:

```
Usuario
   │
   └── Reserva
          │
          └── ReservaAsiento
                 │
                 └── Asiento
                        │
                        └── Sala
                               │
                               └── Funcion
                                      │
                                      └── Pelicula ── PeliculaGenero ── Genero
```

## 💺 Generación automática de asientos
Cuando se crea una sala (`POST /api/sala`), indicando `CantidadFilas` y `CantidadColumnas`, el sistema genera automáticamente todos los asientos correspondientes (por ejemplo, Fila A a la J, Números 1 a N), evitando tener que cargarlos manualmente.

Ejemplo para una sala de 2 filas x 4 columnas:

```
A1 A2 A3 A4
B1 B2 B3 B4
```

## 🎟 Flujo del Sistema
1️⃣ Un Admin crea géneros y películas

2️⃣ Un Admin crea una sala (se generan automáticamente los asientos)

3️⃣ Un Admin crea una función (película + sala + horario), validando que no se solape con otra función existente en esa sala

4️⃣ Un usuario se registra y hace login (recibe un JWT)

5️⃣ El usuario consulta los asientos disponibles de una función

6️⃣ El usuario realiza una reserva de uno o más asientos

## 🔒 Control de Concurrencia
Para evitar reservas duplicadas, el sistema combina dos mecanismos:

**1. Restricción única a nivel de base de datos**, definida en `ReservaAsientoConfig`:

```csharp
builder.HasIndex(ra => new { ra.FuncionId, ra.AsientoId })
    .IsUnique();
```

Esto garantiza que:

```
funcion + asiento = único
```

**2. Transacciones explícitas en el `ReservaServiceImpl`.** Al crear una reserva, se abre una transacción (`BeginTransactionAsync`), se valida en bloque la disponibilidad y capacidad de los asientos solicitados contra los ya ocupados (excluyendo reservas canceladas), y solo si todo es válido se confirma con `CommitAsync`. Ante cualquier error, se ejecuta `RollbackAsync`.

Si dos usuarios intentan reservar el mismo asiento al mismo tiempo:

- el primero se guarda correctamente
- el segundo es rechazado por la validación de asientos ocupados (o por la restricción única de la base de datos si la condición de carrera llega hasta ahí)
- el sistema informa que el asiento ya está reservado

## 🔐 Autenticación y roles
- Autenticación basada en **JWT Bearer Token**.
- Login vía `POST /api/auth/login`, devolviendo un token válido por 2 horas.
- Roles disponibles: `Admin` y `Cliente` (enum `ERole`).
- Los endpoints de administración (`Pelicula`, `Sala`, `Funcion`, `Genero`) están protegidos con `[Authorize(Roles = "Admin")]`, dejando abiertas al público (`[AllowAnonymous]`) las consultas de lectura (listados y detalle).
- El endpoint de reservas (`Reserva`) requiere estar autenticado (`[Authorize]`), sin importar el rol.

## 📚 Documentación de API
La API está documentada usando Swagger / OpenAPI.

Permite:

- visualizar todos los endpoints
- probar requests desde el navegador (incluyendo autenticación con Bearer Token)
- ver modelos de request y response

**Swagger UI (entorno local):**
```
http://localhost:5284/
```

> Swagger UI está configurado como página raíz (`RoutePrefix = string.Empty`).

## ⚠ Manejo Global de Errores
El proyecto implementa un manejador global de excepciones usando la interfaz `IExceptionHandler` de .NET 8 (`GlobalExceptionHandler`), devolviendo respuestas consistentes en formato `ProblemDetails`.

Mapeo de excepciones personalizadas a códigos HTTP:

| Excepción                | Código HTTP |
|---------------------------|-------------|
| `NotFoundException`       | 404 |
| `AlreadyExistsException`  | 409 |
| `ValidationException`     | 400 |
| `BadRequestException`     | 400 |
| Cualquier otra excepción  | 500 |

Esto permite:

- centralizar el manejo de errores
- devolver respuestas consistentes
- evitar duplicación de lógica en los controladores

## ⚙️ Instalación
**1. Clonar repositorio**
```
git clone https://github.com/RobertoTorre96/ApiCine.git
```

**2. Configurar variables (opcional)**

El proyecto usa **SQLite**, por lo que no necesitás levantar un motor de base de datos aparte: la base (`Cine.db`) se crea y se puebla automáticamente al iniciar la aplicación.

Si querés, podés configurar la clave JWT en `appsettings.json` o como variable de entorno:

```json
{
  "JWT_SECRET_KEY": "tu_clave_secreta_de_al_menos_32_caracteres"
}
```

> ⚠️ En cada arranque, la base de datos se elimina y se recrea desde cero (`EnsureDeleted` + `EnsureCreated`), cargando los datos de `Data/Scripts/SeedData.sql`. Esto es intencional para el entorno de desarrollo/demo.

**3. Ejecutar el proyecto**

Con .NET CLI:
```
dotnet run --project ApiCine
```

Al iniciar, se abre automáticamente Swagger UI en:
```
http://localhost:5284/
```

## 📡 Endpoints principales
**Auth**
- `POST /api/auth/login`

**Usuarios**
- `POST /api/usuario`
- `GET /api/usuario` *(Admin)*
- `GET /api/usuario/{id}` *(Admin)*

**Géneros**
- `GET /api/genero`
- `POST /api/genero` *(Admin)*
- `DELETE /api/genero/{id}` *(Admin)*

**Películas**
- `GET /api/pelicula`
- `GET /api/pelicula/{id}`
- `POST /api/pelicula` *(Admin)*

**Salas**
- `GET /api/sala`
- `GET /api/sala/{id}`
- `POST /api/sala` *(Admin)*
- `DELETE /api/sala/{id}` *(Admin)*

**Funciones**
- `GET /api/funcion`
- `GET /api/funcion/{id}`
- `GET /api/funcion/pelicula/{peliculaId}`
- `GET /api/funcion/{funcionId}/asientos-disponibles`
- `POST /api/funcion` *(Admin)*
- `DELETE /api/funcion/{id}` *(Admin)*

**Reservas**
- `POST /api/reserva`
- `GET /api/reserva/{id}`
- `GET /api/reserva/usuario/{usuarioId}`
- `PATCH /api/reserva/{id}/cancelar`

Ejemplo request de reserva:
```json
{
  "funcionId": 1,
  "asientosIds": [5, 6]
}
```
> El `usuarioId` no se envía en el body: se obtiene automáticamente del token JWT del usuario autenticado.

## 🚀 Mejoras Futuras
- Tests unitarios e integración
- Cache para disponibilidad de asientos
- Rate limiting para reservas
- Endpoint de recuperación de contraseña
- Migrar de SQLite a un motor productivo (PostgreSQL / SQL Server) para despliegue

## 👨‍💻 Autor
**Roberto Torre**

💻 GitHub: https://github.com/RobertoTorre96
