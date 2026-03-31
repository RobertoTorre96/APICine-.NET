# 🎬 ApiCine - Cinema Reservation API

**API REST** desarrollada con **.NET 8** y **Entity Framework Core** que implementa un sistema completo de gestión y reservas de asientos para un cine.

El sistema permite administrar:
* **Usuarios** (con seguridad BCrypt)
* **Películas y Géneros**
* **Salas** (con generación automática de asientos)
* **Funciones** (Cartelera)
* **Reservas de asientos** con lógica de estados

Además, implementa un **control de integridad transaccional** para asegurar que las reservas se procesen de forma atómica y segura, evitando la sobreventa de asientos.

---

## 📌 Tabla de Contenidos

* [Descripción](#-descripción)
* [Arquitectura](#-arquitectura)
* [Organización del Proyecto](#-organización-del-proyecto)
* [Tecnologías](#-tecnologías)
* [Modelo del Dominio](#-modelo-del-dominio)
* [Flujo del Sistema](#-flujo-del-sistema)
* [Seguridad y Transacciones](#-seguridad-y-transacciones)
* [Documentación de API](#-documentación-de-api)
* [Manejo Global de Errores](#-manejo-global-de-errores)
* [Instalación](#-instalación)
* [Endpoints Principales](#-endpoints-principales)
* [Mejoras Futuras](#-mejoras-futuras)
* [Autor / Contacto](#-autor--contacto)

---

## 📖 Descripción

Este proyecto simula el backend de un sistema de reservas de cine profesional.
Permite:
* **Registrar usuarios** con almacenamiento seguro de credenciales.
* **Administrar el catálogo** de películas y géneros.
* **Crear salas inteligentes**: el sistema genera automáticamente la grilla de asientos (A1, A2...) según las dimensiones de la sala.
* **Programar funciones** vinculando películas, salas y horarios específicos.
* **Gestión de Reservas**: validación de disponibilidad en tiempo real y cálculo automático de precios.

---

## 🏗 Arquitectura

La aplicación sigue una arquitectura desacoplada en capas, centrada en el **Patrón de Servicio**:

`Controller` → `Service` → `Data (DbContext)` → `Database`

| Capa | Responsabilidad |
| :--- | :--- |
| **Controller** | Exponer endpoints REST y manejar el protocolo HTTP. |
| **Service** | Orquestar la lógica de negocio y validaciones. |
| **DTOs** | Objetos de transferencia para entrada y salida de datos (Data Privacy). |
| **Mappers** | Conversión automática entre Entidades y DTOs (AutoMapper). |
| **Data** | Modelado de base de datos y persistencia con EF Core. |

---

## 📦 Organización del Proyecto

El proyecto está organizado por **Features** (Feature-based structure), facilitando la escalabilidad y mantenibilidad:

```
ApiCine
│
├── Features
│   ├── Usuario
│   │   ├── DTOs, Service, UsuarioEntity.cs
│   ├── Pelicula
│   │   ├── DTOs, Service, PeliculaEntity.cs
│   ├── Reserva
│   │   ├── DTOs, Service, ReservaEntity.cs
│   └── ... (Sala, Funcion, Genero)
│
├── Data (AppDbContext & Migrations)
├── Exceptions (Custom Exceptions & GlobalExceptionHandler)
├── Mappers (MappingProfile - AutoMapper)
└── Controllers (API Endpoints)

```
---
⚙️ Tecnologías:

Backend: C# / .NET 8

ORM: Entity Framework Core

Base de Datos: SQLite (desarrollo) / SQL Server (producción)

Mapeo: AutoMapper

Seguridad: BCrypt.Net-Next (Hashing de contraseñas)

Documentación: Swagger / OpenAPI

Herramientas: Visual Studio 2022, Postman, dotnet-ef tools

---
🧩 Modelo del Dominio:

El sistema utiliza relaciones complejas para representar la realidad de un cine:

Muchos a Muchos: Película ↔ Género (vía PeliculaGenero), Reserva ↔ Asiento (vía ReservaAsiento).

Uno a Muchos: Sala → Asientos, Película → Funciones, Usuario → Reservas.

---
🔒 Seguridad y Transacciones:

Control de Integridad

Para las reservas, se utiliza IDbContextTransaction. Esto garantiza la atomicidad: si la validación de un solo asiento falla, la transacción hace un rollback completo, evitando reservas parciales o corruptas.

Protección de Datos
Passwords: Implementación de BCrypt para evitar el almacenamiento de texto plano.

Validación de Unicidad: Filtros asincrónicos para Email y Username antes de persistir nuevos usuarios.

---
📚 Documentación de API:

La API está totalmente documentada con Swagger. Permite visualizar los modelos de datos y probar los endpoints directamente desde el navegador.

Swagger UI: https://localhost:XXXX/swagger/index.html

---
⚠ Manejo Global de Errores:

Se implementó un middleware de Global Exception Handler utilizando AddExceptionHandler.

Centraliza el manejo de excepciones personalizadas (NotFoundException, BadRequestException).

Devuelve respuestas consistentes con códigos de estado HTTP precisos (400, 404, 500).

Mantiene los controladores limpios de bloques try-catch.

---
⚙️ Instalación:

Clonar el repositorio

Bash

git clone https://github.com/RobertoTorre96/APICine-.NET.git
cd APICine-.NET

dotnet restore


Ejecutar Migraciones;
dotnet ef database update

Correr el proyecto:

dotnet run --project ApiCine

---
📡 Endpoints Principales
Usuarios: POST /api/Usuario (Registro), GET /api/Usuario/{id}

Películas: POST /api/Pelicula, GET /api/Pelicula

Salas: POST /api/Sala (Generación de asientos automática)

Funciones: GET /api/Funcion (Cartelera detallada)

Reservas: POST /api/Reserva (Permite múltiples asientos en una sola operación)

---
🚀 Mejoras Futuras:

[ ] Implementación de JWT (JSON Web Tokens) para autenticación y roles.

[ ] Unit Testing con xUnit y Moq para la lógica de reservas.

[ ] Implementación de Soft Delete para registros históricos.

---
# 👨‍💻 Autor:


**Roberto Torre**

📧 Email
[torreroberto1996@gmail.com](mailto:torreroberto1996@gmail.com)

💻 GitHub
[https://github.com/RobertoTorre96](https://github.com/RobertoTorre96)
