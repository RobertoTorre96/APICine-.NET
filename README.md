¡Por supuesto! Aquí tienes el README.md adaptado a las tecnologías y estructura de tu proyecto .NET, siguiendo exactamente el formato que solicitaste:🎬 ApiCine - Cinema Reservation APIAPI REST desarrollada con .NET 8 que implementa un sistema completo de reservas de asientos para un cine.  El sistema permite administrar:  usuarios  películas  géneros  salas  funciones  reservas de asientos  Además implementa control de concurrencia para evitar que dos usuarios reserven el mismo asiento para la misma función, utilizando índices únicos en la base de datos.  Este proyecto fue desarrollado como práctica de backend con C#, ASP.NET Core, Entity Framework Core y buenas prácticas de arquitectura.  📌 Tabla de ContenidosDescripciónDatos de pruebaArquitecturaOrganización del proyectoTecnologíasModelo del dominioFlujo del sistemaControl de concurrenciaDocumentación de APIManejo global de erroresInstalaciónEndpoints principalesMejoras futurasAutor / Contacto📖 DescripciónEste proyecto simula el backend de un sistema de reservas de cine.  Permite:registrar usuarios con roles de Cliente y Admin  administrar películas y asociarlas a múltiples géneros  crear salas con generación automática de asientos según filas y columnas  programar funciones con validación de solapamiento de horarios  reservar asientos específicos para una función  El sistema está diseñado para evitar problemas de concurrencia en reservas, uno de los problemas más comunes en sistemas de tickets.  🚀 Datos de pruebaLa aplicación carga automáticamente un script de inicialización (SeedData.sql) al arrancar el proyecto, poblando la base de datos con géneros, películas, salas y usuarios base.  🔑 Usuarios de pruebaPodés autenticarte en el endpoint /api/auth/login con cualquiera de estos usuarios ya cargados:  UsernameContraseñaRolEmailadminadminAdminadminjuan_perezUser123*Clientejuan@gmail.commaria_cineUser123*Clientemaria@gmail.comEl usuario Admin tiene acceso a los endpoints de administración (crear películas, salas, funciones, etc.).  Los usuarios Cliente permiten probar el flujo de reserva de asientos de punta a punta.  SQLINSERT INTO Usuario (Id, Username, Nombre, Email, Password, Role)
VALUES (
    1,
    'admin',
    'Admin General',
    'admin',    
    '$2a$11$iKApQgertluPRENCUQLYE.KgOeJjCnENblNYPRibDLoQPOr4p7HQ.',--admin
    'Admin'
);

INSERT INTO Usuario (Id, Username, Nombre, Email, Password, Role) 
VALUES (2, 'juan_perez', 'Juan Perez', 'juan@gmail.com', '$2a$11$M.Jz02Z.IP7LOMyBb70NS.pUopbrwmDRllulLrIoUHS0oO/0YvY8W', 'Cliente');-- 12345678

INSERT INTO Usuario (Id, Username, Nombre, Email, Password, Role) 
VALUES (3, 'maria_cine', 'Maria Garcia', 'maria@gmail.com', '$2a$11$M.Jz02Z.IP7LOMyBb70NS.pUopbrwmDRllulLrIoUHS0oO/0YvY8W', 'Cliente');-- 12345678
⚠️ Estas son credenciales de un entorno de prueba, no de producción real. Las contraseñas ya están hasheadas con BCrypt en la base de datos.  🏗 ArquitecturaLa aplicación sigue una arquitectura en capas adaptada al ecosistema .NET:  Controller
   ↓
Service
   ↓
DbContext (Entity Framework)
   ↓
Entity
   ↓
Database (SQLite)
Cada capa tiene responsabilidades claras.  CapaResponsabilidadControllerExponer endpoints REST y autorizar peticionesServiceLógica de negocio y validacionesDbContextAcceso a base de datos y configuraciones ORMEntityModelo de dominio📦 Organización del ProyectoEl proyecto está organizado por features (feature-based packaging) dentro de la carpeta Features.  En lugar de tener carpetas globales como Services o Controllers, cada entidad del dominio tiene su propia carpeta.  Esto facilita:  encontrar rápidamente la lógica relacionada  mantener el proyecto escalable  evitar carpetas demasiado grandes  mejorar la mantenibilidad  Estructura simplificada:  ApiCine
│
├── Features
│   ├── Auth
│   ├── Usuario
│   │   ├── Controller
│   │   ├── DTOs
│   │   ├── Service
│   │   └── UsuarioEntity.cs
│   ├── Pelicula
│   ├── Reserva
│   ├── Sala
│   ├── Funcion
│   └── Genero
│
├── Data
│   └── AppDbContext.cs
│
├── Exceptions
│   └── GlobalExceptionHandler.cs
│
└── Mappers
    └── MappingProfile.cs
⚙️ TecnologíasBackendC# / .NET 8.0  ASP.NET Core  Entity Framework Core  AutoMapper  JWT Bearer Authentication  BCrypt.Net  DocumentaciónSwagger / Swashbuckle  Base de datosSQLite  🧩 Modelo del DominioEntidades principales:  Usuario  Pelicula  Genero  PeliculaGenero  Sala  Asiento  Funcion  Reserva  ReservaAsiento  Relación simplificada:  Usuario
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
                                      └── Pelicula
💺 Generación automática de asientosCuando se crea una sala mediante el endpoint, el sistema genera automáticamente los asientos en la base de datos combinando letras para las filas y números para las columnas.  Ejemplo:  A1 A2 A3 A4
B1 B2 B3 B4
C1 C2 C3 C4
Esto permite crear dinámicamente salas de diferentes tamaños (estableciendo la cantidad de filas y columnas).  🎟 Flujo del Sistema1️⃣ Se crea una película  2️⃣ Se crea una sala (se generan automáticamente los asientos)  3️⃣ Se crea una función (película + sala + horario)  4️⃣ Los usuarios realizan reservas de asientos  🔒 Control de ConcurrenciaPara evitar reservas duplicadas se utiliza una restricción de índice único en Entity Framework Core:  C#builder.HasIndex(ra => new { ra.FuncionId, ra.AsientoId })
       .IsUnique()
       .HasDatabaseName("IX_ReservaAsiento_FuncionId_AsientoId");
Esto garantiza en la base de datos que:  funcion + asiento = único
Si dos usuarios intentan reservar el mismo asiento al mismo tiempo:  el primero se guarda correctamente  el segundo produce una excepción (detectada por el sistema)  el sistema informa que el asiento ya está reservado (Conflicto 409)  📚 Documentación de APILa API está documentada usando Swagger.  Permite:  visualizar todos los endpoints  probar requests desde el navegador incluyendo el Token JWT  ver modelos de request y response  Swagger UI (entorno local):http://localhost:5284/swagger
⚠ Manejo Global de ErroresEl proyecto implementa un Global Exception Handler usando la interfaz IExceptionHandler.  Esto permite:  centralizar el manejo de errores en un solo componente  mapear excepciones personalizadas (NotFoundException, AlreadyExistsException, BadRequestException) a códigos HTTP correctos (404, 409, 400)  devolver respuestas consistentes bajo el estándar ProblemDetails  evitar duplicación de lógica try-catch en los controladores  ⚙️ Instalación1. Clonar repositorioBashgit clone https://github.com/RobertoTorre96/cinema-api.git
(Asegurate de usar la URL correcta de tu repo)2. Configuración de Base de Datos
El proyecto usa SQLite por defecto, por lo que no necesitas instalar un motor externo. La configuración ya se encuentra en appsettings.json:  JSON"ConnectionStrings": {
  "DefaultConnection": "Data Source=Cine.db"
}
3. Ejecutar el proyecto
Posiciónate en la carpeta del proyecto y ejecuta:  Bashdotnet run
💡 El sistema creará automáticamente el archivo Cine.db y ejecutará el script SeedData.sql para tener todo listo para probar.  📡 Endpoints principalesAutenticaciónPOST /api/auth/login  UsuariosPOST /api/usuario  GET /api/usuario  PelículasPOST /api/pelicula  GET /api/pelicula  GET /api/pelicula/{id}  SalasPOST /api/sala  GET /api/sala  FuncionesPOST /api/funcion  GET /api/funcion  GET /api/funcion/{funcionId}/asientos-disponibles  ReservasPOST /api/reserva  PATCH /api/reserva/{id}/cancelar  Ejemplo request para reservar:  JSON{
  "funcionId": 3,
  "asientosIds": [5, 6]
}
🚀 Mejoras FuturasTests unitarios con xUnit/NUnitCache para disponibilidad de asientosRate limiting para reservasPaginación dinámica en todos los endpoints👨‍💻 AutorRoberto Torre📧 Email: torreroberto1996@gmail.com💻 GitHub: https://github.com/RobertoTorre96
