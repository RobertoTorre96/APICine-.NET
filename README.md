# 🎬 ApiCine - Cinema Reservation API

API REST desarrollada con .NET 8 y Entity Framework Core que implementa un sistema completo de gestión y reservas de asientos para un cine, con enfoque en integridad transaccional, concurrencia y seguridad.

---

## 🔗 Live Demo

Swagger UI disponible en:

https://apicine-net.onrender.com/swagger/index.html

---

## 🚀 Key Features

### 🎟️ Gestión de Reservas

Sistema de reservas que permite seleccionar múltiples asientos en una sola operación.

### 🔒 Seguridad

* Autenticación mediante JWT
* Hashing seguro de contraseñas con BCrypt
* Validación de unicidad para usuarios

### ⚡ Integridad Transaccional y Concurrencia

Implementación de `IDbContextTransaction` para garantizar atomicidad:
si falla la reserva de un asiento, se realiza rollback completo evitando inconsistencias y sobreventa.

### 🪑 Generación Automática de Asientos

Creación dinámica de grillas de asientos (A1, A2...) según dimensiones de cada sala.

### 🧩 Arquitectura Desacoplada

Arquitectura organizada por Features y patrón de servicios:

* Controllers
* Services
* DTOs
* Data Layer

### ⚠️ Manejo Global de Errores

Middleware centralizado para respuestas HTTP consistentes y controladores limpios de bloques try-catch.

### 📖 Documentación Interactiva

Integración completa con Swagger/OpenAPI para probar endpoints directamente desde el navegador.

---

## 🛠️ Stack Tecnológico

### Backend

* C#
* .NET 8 / ASP.NET Core

### Persistencia

* Entity Framework Core
* SQLite (desarrollo)
* SQL Server (producción)

### Seguridad

* JWT Authentication
* BCrypt.Net-Next

### Arquitectura y Herramientas

* AutoMapper
* DTO Pattern
* Swagger / OpenAPI

---

## 🏗️ Modelo del Dominio

El sistema implementa relaciones complejas para representar escenarios reales:

### Relaciones Muchos a Muchos

* Películas ↔ Géneros
* Reservas ↔ Asientos

### Relaciones Uno a Muchos

* Sala → Asientos
* Película → Funciones
* Usuario → Reservas

---

## 🚦 Cómo Probar la API

### Clonar el repositorio

```bash
git clone https://github.com/RobertoTorre96/APICine-.NET.git
cd APICine-.NET
```

### Restaurar dependencias

```bash
dotnet restore
```

### Ejecutar migraciones

```bash
dotnet ef database update
```

### Ejecutar la aplicación

```bash
dotnet run --project ApiCine
```

---

## 🔐 Credenciales de prueba

### Usuario Administrador

```json
{
  "email": "admin",
  "password": "admin"
}
```

---

## 📡 Endpoints Principales

### Usuarios

* `POST /api/Usuario`
* `GET /api/Usuario/{id}`

### Películas

* `POST /api/Pelicula`
* `GET /api/Pelicula`

### Salas

* `POST /api/Sala`

### Funciones

* `GET /api/Funcion`

### Reservas

* `POST /api/Reserva`

---

## 🚀 Mejoras Futuras

* [ ] Unit Testing con xUnit y Moq
* [ ] Refresh Tokens y autorización basada en roles
* [ ] Deploy con base de datos persistente
* [ ] CI/CD con GitHub Actions

---

## 👨‍💻 Autor

Edmundo Roberto Torre

📧 Email: [torreroberto1996@gmail.com](mailto:torreroberto1996@gmail.com)

💻 GitHub:
https://github.com/RobertoTorre96
