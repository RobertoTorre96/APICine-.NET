# 🎬 ApiCine

API REST para la gestión de un sistema de cine: películas, géneros, salas, funciones, asientos, reservas de usuarios y autenticación con JWT.

Construida con **.NET 8**, **Entity Framework Core** (SQLite) y **AutoMapper**, siguiendo una arquitectura por *features* (vertical slices).

🔗 **Demo desplegada en Render:** https://apicine-net.onrender.com

---

## 📌 Tabla de contenidos

- [Tecnologías](#-tecnologías)
- [Arquitectura del proyecto](#-arquitectura-del-proyecto)
- [Requisitos previos](#-requisitos-previos)
- [Instalación y ejecución local](#-instalación-y-ejecución-local)
- [Variables de entorno](#-variables-de-entorno)
- [Autenticación](#-autenticación)
- [Endpoints de la API](#-endpoints-de-la-api)
- [Modelo de datos](#-modelo-de-datos)
- [Manejo de errores](#-manejo-de-errores)
- [🧪 Probar la API con Swagger](#-probar-la-api-con-swagger)
- [Despliegue en Render](#-despliegue-en-render)

---

## 🛠 Tecnologías

| Tecnología | Uso |
|---|---|
| .NET 8 / ASP.NET Core Web API | Framework principal |
| Entity Framework Core 8 + SQLite | Persistencia de datos |
| AutoMapper 14 | Mapeo entre Entidades y DTOs |
| JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer) | Autenticación y autorización por roles |
| BCrypt.Net-Next | Hash seguro de contraseñas |
| Swashbuckle / Swagger | Documentación interactiva de la API |
| Render | Hosting / despliegue |

---

## 🏗 Arquitectura del proyecto

El proyecto está organizado por **features** (módulos de dominio), no por capas técnicas. Cada feature agrupa su Controller, Entity, DTOs, Service/Interface y Config de EF Core:

```
ApiCine/
├── Features/
│   ├── Auth/            → Login y generación de JWT
│   ├── Usuario/          → Registro y consulta de usuarios
│   ├── Pelicula/         → CRUD de películas
│   ├── Genero/           → CRUD de géneros
│   ├── Sala/             → Salas y generación automática de asientos
│   ├── Asiento/          → Asientos de cada sala
│   ├── Funcion/          → Funciones (horarios de proyección)
│   ├── Reserva/          → Reservas de asientos por función
│   ├── Relaciones/        → Tablas intermedias (PeliculaGenero, ReservaAsiento)
│   └── Enums/ , Role/     → Enumerados (EEstadoReserva, ERole)
├── common/DTOs/           → DTOs compartidos (paginación)
├── Data/                  → AppDbContext y Scripts/SeedData.sql
├── Exceptions/            → Excepciones custom + GlobalExceptionHandler
├── Mappers/               → MappingProfile (AutoMapper)
└── Program.cs             → Configuración de la app (JWT, Swagger, DI, seed)
```

Al iniciar, la aplicación **recrea la base de datos SQLite** (`EnsureDeleted` + `EnsureCreated`) y ejecuta un script `SeedData.sql` con datos iniciales de prueba.

---

## ✅ Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Un editor como Visual Studio 2022 o VS Code

---

## 🚀 Instalación y ejecución local

```bash
# 1. Cloná el repositorio
git clone <url-del-repositorio>
cd ApiCine

# 2. Restaurá los paquetes
dotnet restore

# 3. Ejecutá la aplicación
dotnet run --project ApiCine
```

Por defecto la API queda disponible en:

- HTTP: `http://localhost:5284`
- HTTPS: `https://localhost:7043`

Al levantar el proyecto en modo `Development`, el navegador se abre automáticamente en `/swagger`. En este proyecto Swagger UI está montado en la **raíz** (`RoutePrefix = string.Empty`), por lo que también se puede acceder directo desde `/`.

---

## 🔑 Variables de entorno

| Variable | Descripción | Valor por defecto (dev) |
|---|---|---|
| `JWT_SECRET_KEY` | Clave secreta usada para firmar los tokens JWT | Definida en `appsettings.json` / `launchSettings.json` |
| `ASPNETCORE_ENVIRONMENT` | Entorno de ejecución (`Development` / `Production`) | `Development` |

> ⚠️ En producción (Render), `JWT_SECRET_KEY` debe configurarse como variable de entorno del servicio, **no** dejarse hardcodeada.

---

## 🔐 Autenticación

La API usa **JWT Bearer**. El flujo típico es:

1. Registrar un usuario: `POST /api/usuario`
2. Iniciar sesión: `POST /api/auth/login` → devuelve un `token`
3. Enviar ese token en el header `Authorization: Bearer <token>` para los endpoints protegidos

El token incluye los claims: `username`, `email`, `role` y `userId`, y expira a las **2 horas**.

### Roles (`ERole`)

| Valor | Rol |
|---|---|
| `1` | Cliente |
| `2` | Admin |

- Solo un usuario **Admin autenticado** puede registrar a otro usuario con rol `Admin`.
- Los endpoints administrativos (crear/eliminar Película, Género, Sala, Función) requieren rol `Admin`.

---

## 📚 Endpoints de la API

> 🔒 = requiere JWT · 👑 = requiere rol **Admin** · 🌐 = público (`AllowAnonymous`)

### Auth (`/api/auth`)

| Método | Ruta | Descripción | Acceso |
|---|---|---|---|
| POST | `/api/auth/login` | Login con email y password, devuelve JWT | 🌐 |

### Usuario (`/api/usuario`)

| Método | Ruta | Descripción | Acceso |
|---|---|---|---|
| POST | `/api/usuario` | Registra un nuevo usuario (Cliente o Admin) | 🌐 |
| GET | `/api/usuario/{id}` | Obtiene un usuario por ID | 👑 |
| GET | `/api/usuario` | Lista todos los usuarios | 👑 |

### Película (`/api/pelicula`)

| Método | Ruta | Descripción | Acceso |
|---|---|---|---|
| POST | `/api/pelicula` | Crea una película (con géneros asociados) | 👑 |
| GET | `/api/pelicula?numeroPagina=1&tamPagina=10` | Lista paginada de películas | 🌐 |
| GET | `/api/pelicula/{id}` | Obtiene una película por ID | 🌐 |

### Género (`/api/genero`)

| Método | Ruta | Descripción | Acceso |
|---|---|---|---|
| GET | `/api/genero` | Lista todos los géneros | 🌐 |
| POST | `/api/genero` | Crea un género | 👑 |
| DELETE | `/api/genero/{id}` | Elimina un género | 👑 |

### Sala (`/api/sala`)

| Método | Ruta | Descripción | Acceso |
|---|---|---|---|
| POST | `/api/sala` | Crea una sala (genera automáticamente sus asientos según filas/columnas) | 👑 |
| GET | `/api/sala` | Lista todas las salas | 🌐 |
| GET | `/api/sala/{id}` | Obtiene una sala por ID | 🌐 |
| DELETE | `/api/sala/{id}` | Elimina una sala (falla si tiene funciones asociadas) | 👑 |

### Función (`/api/funcion`)

| Método | Ruta | Descripción | Acceso |
|---|---|---|---|
| POST | `/api/funcion` | Crea una función (valida solapamiento de horarios en la misma sala) | 👑 |
| GET | `/api/funcion` | Lista todas las funciones | 🌐 |
| GET | `/api/funcion/{id}` | Obtiene el detalle de una función | 🌐 |
| GET | `/api/funcion/pelicula/{peliculaId}` | Lista funciones futuras de una película | 👑 |
| GET | `/api/funcion/{funcionId}/asientos-disponibles` | Lista los asientos disponibles para esa función | 👑 |
| DELETE | `/api/funcion/{id}` | Elimina una función (falla si ya tiene reservas) | 👑 |

### Reserva (`/api/reserva`)

| Método | Ruta | Descripción | Acceso |
|---|---|---|---|
| POST | `/api/reserva` | Crea una reserva de asientos para una función (usuario autenticado extraído del token) | 🔒 |
| GET | `/api/reserva/{id}` | Obtiene una reserva por ID | 🔒 |
| GET | `/api/reserva/usuario/{usuarioId}` | Lista las reservas de un usuario | 🔒 |
| PATCH | `/api/reserva/{id}/cancelar` | Cancela una reserva | 🔒 |

---

## 🗃 Modelo de datos

Entidades principales y sus relaciones:

- **Pelicula** ↔ **Genero** (muchos a muchos, vía `PeliculaGenero`)
- **Sala** → **Asiento** (una sala tiene muchos asientos, generados automáticamente)
- **Pelicula** + **Sala** → **Funcion** (una función combina una película, una sala y un horario)
- **Usuario** + **Funcion** → **Reserva** (una reserva pertenece a un usuario y a una función)
- **Reserva** ↔ **Asiento** (muchos a muchos, vía `ReservaAsiento`, ligado también a la `Funcion`)

Estados de reserva (`EEstadoReserva`): `Pendiente`, `Confirmada`, `Cancelada`.

---

## ⚠️ Manejo de errores

La API centraliza el manejo de errores con un `GlobalExceptionHandler` y `ProblemDetails`, devolviendo respuestas consistentes para:

- `NotFoundException` → 404
- `BadRequestException` / `ValidationException` → 400
- `AlreadyExistsException` → 409 (conflicto, ej. género o sala duplicada)

---

## 🧪 Probar la API con Swagger

La forma más rápida de explorar y probar todos los endpoints es a través de **Swagger UI**, ya desplegado junto con la API en Render:

👉 **https://apicine-net.onrender.com/index.html**

Pasos sugeridos para probar el flujo completo:

1. Entrá al link de Swagger de arriba.
2. Expandí **`POST /api/usuario`** y registrá un usuario (podés usar `"Role": 1` para Cliente).
3. Expandí **`POST /api/auth/login`**, enviá el email y password del usuario creado, y copiá el `token` de la respuesta.
4. Hacé clic en el botón **`Authorize`** (🔒) en la parte superior de Swagger y pegá el token con el formato:
   ```
   Bearer <tu_token_aquí>
   ```
5. Ya podés probar los endpoints protegidos (crear salas, funciones, reservas, etc. según tu rol).

> 💡 Nota: al estar en el plan gratuito de Render, el servicio puede "dormirse" tras un período de inactividad. La primera petición luego de estar inactivo puede tardar unos segundos extra en responder mientras la instancia se reactiva.

---

## ☁️ Despliegue en Render

El proyecto está desplegado como **Web Service** en [Render](https://render.com):

- **Runtime:** Docker / .NET 8
- **Base de datos:** SQLite embebida (se recrea y siembra automáticamente en cada arranque del contenedor)
- **Swagger UI:** montado en la raíz del sitio (`/`)

Variables de entorno configuradas en el servicio de Render:

| Variable | Valor |
|---|---|
| `JWT_SECRET_KEY` | Clave secreta de firma de JWT (definida en el panel de Render) |
| `ASPNETCORE_ENVIRONMENT` | `Production` |

> ℹ️ Al usar SQLite con `EnsureDeleted()/EnsureCreated()` en el arranque, los datos **no persisten entre reinicios/redeploys** del servicio — cada arranque parte de los datos definidos en `SeedData.sql`.

---

## 📄 Licencia

Proyecto académico / de práctica. Libre uso para fines de aprendizaje.
