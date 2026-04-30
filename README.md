🎬ApiCine - Cinema Reservation API 
API REST desarrollada con .NET 8 y Entity Framework Core que implementa un sistema completo de gestión y reservas de asientos para un cine, con enfoque en integridad transaccional y seguridad.  

🚀 Key Features
Gestión de Reservas: Sistema atómico que permite reservar múltiples asientos en una sola operación.  

Generación Automática de Salas: Creación dinámica de grillas de asientos (A1, A2...) basada en las dimensiones de la sala.  

Integridad Transaccional: Implementación de IDbContextTransaction para asegurar la atomicidad: si falla la reserva de un asiento, se realiza un rollback completo para evitar sobreventa.  

Seguridad: Hashing de contraseñas con BCrypt y validación de unicidad de datos sensibles.  

Manejo Global de Errores: Middleware centralizado para respuestas HTTP consistentes y controladores limpios de bloques try-catch.  

🛠️ Stack Tecnológico
Backend: C# / .NET 8  

ORM: Entity Framework Core  

Base de Datos: SQLite (desarrollo) / SQL Server (producción)  

Mapeo: AutoMapper (Patrón DTO)  

Seguridad: BCrypt.Net-Next  

Documentación: Swagger / OpenAPI  

🏗️ Arquitectura y Organización
El proyecto sigue una arquitectura desacoplada organizada por Features, facilitando la escalabilidad:  

Patrón de Servicio: Capa de lógica de negocio independiente de los controladores.  

Data Privacy: Uso estricto de DTOs para la transferencia de datos de entrada y salida.  

Modelo de Dominio: Relaciones complejas (Muchos a Muchos) para Películas/Géneros y Reservas/Asientos.  

🚦 Cómo Probar la API
El proyecto incluye un Seeder automático con datos de prueba para facilitar la evaluación inmediata.  

1. Instalación:

En Bash u otra terminal:
'''
git clone https://github.com/RobertoTorre96/APICine-.NET.git
cd APICine-.NET
dotnet restore
dotnet ef database update
dotnet run --project ApiCine
'''
2. Credenciales de Acceso (Admin):
Para probar los endpoints protegidos en Swagger:

Email: admin

  

Password: admin*

  

3. Documentación Interactiva:
Una vez en ejecución, accede a: http://localhost:5284/swagger/index.html

  

🚀 Mejoras Futuras
[ ] Implementación de JWT para autenticación y roles.  

[ ] Unit Testing con xUnit y Moq para la lógica de transacciones.  

[ ] Implementación de Soft Delete para registros históricos.  

👨‍💻 Autor
Edmundo Roberto Torre

Email: torreroberto1996@gmail.com  

GitHub: RobertoTorre96
