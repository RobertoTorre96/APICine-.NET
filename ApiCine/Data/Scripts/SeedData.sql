-- 1. GENEROS
INSERT INTO Genero (Id, Nombre) VALUES (1, 'Acción');
INSERT INTO Genero (Id, Nombre) VALUES (2, 'Ciencia Ficción');
INSERT INTO Genero (Id, Nombre) VALUES (3, 'Terror');
INSERT INTO Genero (Id, Nombre) VALUES (4, 'Drama');

-- 2. PELICULAS
INSERT INTO Pelicula (Id, Codigo, Titulo, Duracion, Sinopsis) 
VALUES (1, 'PEL-001', 'Inception', 148, 'Un ladrón que roba secretos a través del subconsciente.');
INSERT INTO Pelicula (Id, Codigo, Titulo, Duracion, Sinopsis) 
VALUES (2, 'PEL-002', 'The Dark Knight', 152, 'Batman se enfrenta al Joker en Gotham City.');
INSERT INTO Pelicula (Id, Codigo, Titulo, Duracion, Sinopsis) 
VALUES (3, 'PEL-003', 'Interstellar', 169, 'Un equipo de exploradores viaja a través de un agujero de gusano.');

-- 3. RELACIÓN PELICULA-GENERO (Tabla Intermedia)
INSERT INTO Pelicula_Genero (Id, PeliculaId, GeneroId) VALUES (1, 1, 2); -- Inception - SciFi
INSERT INTO Pelicula_Genero (Id, PeliculaId, GeneroId) VALUES (2, 2, 1); -- Batman - Acción
INSERT INTO Pelicula_Genero (Id, PeliculaId, GeneroId) VALUES (3, 2, 4); -- Batman - Drama
INSERT INTO Pelicula_Genero (Id, PeliculaId, GeneroId) VALUES (4, 3, 2); -- Interstellar - SciFi

-- 4. SALAS
INSERT INTO Sala (Id, Cod, Nombre, CantidadFilas, CantidadColumnas) 
VALUES (1, 'SALA-A', 'IMAX 3D', 10, 10);
INSERT INTO Sala (Id, Cod, Nombre, CantidadFilas, CantidadColumnas) 
VALUES (2, 'SALA-B', 'Premium Dolby', 5, 8);

-- 5. ASIENTOS (Algunos de prueba para la Sala 1)
INSERT INTO Asiento (Id, Fila, Numero, SalaId) VALUES (1, 'A', 1, 1);
INSERT INTO Asiento (Id, Fila, Numero, SalaId) VALUES (2, 'A', 2, 1);
INSERT INTO Asiento (Id, Fila, Numero, SalaId) VALUES (3, 'B', 1, 1);
INSERT INTO Asiento (Id, Fila, Numero, SalaId) VALUES (4, 'B', 2, 1);
-- Asientos para la Sala 2
INSERT INTO Asiento (Id, Fila, Numero, SalaId) VALUES (5, 'A', 1, 2);

-- 6. USUARIOS (Ya tienes el Admin en HasData, estos son clientes de prueba)
-- Las contraseñas son el hash de "User123*"
INSERT INTO Usuario (Id, Username, Nombre, Email, Password, Role)
VALUES (
    1,
    'admin',
    'Admin General',
    'admin',    
    '$2a$11$JUa6WFSdq3YSLaEiebLDk.NffdIv2CIAOYtyvz3ZKSc1yxhwcdJU6',
    'Admin'
);

INSERT INTO Usuario (Id, Username, Nombre, Email, Password, Role) 
VALUES (2, 'juan_perez', 'Juan Perez', 'juan@gmail.com', '$2a$11$qR77E6A.F3E6W1fN.O5.O.UfU6M6u5V8eYf.Wk2m9Z6Y7xW1L2Z2.', 'Cliente');
INSERT INTO Usuario (Id, Username, Nombre, Email, Password, Role) 
VALUES (3, 'maria_cine', 'Maria Garcia', 'maria@gmail.com', '$2a$11$qR77E6A.F3E6W1fN.O5.O.UfU6M6u5V8eYf.Wk2m9Z6Y7xW1L2Z2.', 'Cliente');

-- 7. FUNCIONES (Programadas para el futuro)
INSERT INTO Funcion (Id, Precio, FechaHora, PeliculaId, SalaId) 
VALUES (1, 5500.00, '2026-06-01 20:00:00', 1, 1);
INSERT INTO Funcion (Id, Precio, FechaHora, PeliculaId, SalaId) 
VALUES (2, 4800.00, '2026-06-01 22:30:00', 2, 1);
INSERT INTO Funcion (Id, Precio, FechaHora, PeliculaId, SalaId) 
VALUES (3, 6000.00, '2026-06-02 18:00:00', 3, 2);

-- 8. RESERVAS
INSERT INTO Reserva (Id, Cod, Estado, Fecha, FuncionId, UsuarioId) 
VALUES (1, 'RES-9901', 'Confirmada', '2026-04-01 10:00:00', 1, 2);

-- 9. RESERVA-ASIENTO (Vínculo de qué asiento se ocupó en qué reserva/función)
INSERT INTO Reserva_asiento (Id, ReservaId, AsientoId, FuncionId) 
VALUES (1, 1, 1, 1); -- Juan reservó el Asiento A1 en la Función 1
INSERT INTO Reserva_asiento (Id, ReservaId, AsientoId, FuncionId) 
VALUES (2, 1, 2, 1); -- Juan reservó también el Asiento A2
