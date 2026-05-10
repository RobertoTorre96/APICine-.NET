    using ApiCine.Features.Funcion;
    using ApiCine.Features.Funcion.DTOs;
    using ApiCine.Features.Genero;
    using ApiCine.Features.Genero.DTOs;
    using ApiCine.Features.Pelicula;
    using ApiCine.Features.Pelicula.DTOs;
    using ApiCine.Features.Reserva;
    using ApiCine.Features.Reserva.DTOs;
    using ApiCine.Features.Role;
    using ApiCine.Features.Sala;
    using ApiCine.Features.Sala.DTOs;
    using ApiCine.Features.Usuario;
    using ApiCine.Features.Usuario.DTOs;
    using AutoMapper;
    using System.Linq;

    namespace ApiCine.Mappers {
        public class MappingProfile : Profile {
            public MappingProfile() {

                CreateMap<GeneroEntity, GeneroResponseDto>();
                CreateMap<GeneroRequestDto, GeneroEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // El ID lo genera la DB
                .ForMember(dest => dest.PeliculaGeneros, opt => opt.Ignore());





                CreateMap<PeliculaRequestDto, PeliculaEntity>()
                // Ignoramos el Id porque lo genera la base de datos
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                // Ignoramos el Codigo porque lo generaremos por lógica (ej: PEL-001)
                .ForMember(dest => dest.Codigo, opt => opt.Ignore())
                // IMPORTANTE: Ignoramos la colección de géneros aquí.
                // Los IDs que vienen en el DTO los procesaremos en el Service.
                .ForMember(dest => dest.PeliculaGeneros, opt => opt.Ignore())
                .ForMember(dest => dest.Funciones, opt => opt.Ignore());

                // 2. De Entidad a Response (Para MOSTRAR / GET)
                CreateMap<PeliculaEntity, PeliculaResponseDto>()
                    // Transformamos la tabla intermedia en una lista simple de nombres
                    .ForMember(dest => dest.Generos, opt => opt.MapFrom(src =>
                        src.PeliculaGeneros
                           .Select(pg => pg.Genero.Nombre)
                           .ToList()));




                CreateMap<SalaRequestDto, SalaEntity>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    // El 'Cod' lo podrías generar vos o mapear desde el Nombre si usás lógica similar
                    .ForMember(dest => dest.Cod, opt => opt.MapFrom(src => src.Nombre.ToUpper()))
                    .ForMember(dest => dest.Asientos, opt => opt.Ignore())
                    .ForMember(dest => dest.Funciones, opt => opt.Ignore());


                CreateMap<SalaEntity, SalaResponseDto>();
            


                // --- BLOQUE USUARIO ---

                // 1. De Request a Entidad (Registro de usuario)
                CreateMap<UsuarioRequestDto, UsuarioEntity>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    // Mapeamos el int que viene del DTO al Enum de la Entidad
                    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (ERole)src.Role))
                    .ForMember(dest => dest.Reservas, opt => opt.Ignore());

                // 2. De Entidad a Response (Perfil de usuario / Login)
                CreateMap<UsuarioEntity, UsuarioResponseDto>()
                    // Convertimos el Enum a String para que el Frontend lea "Admin" o "Cliente"
                    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

                // --- BLOQUE RESERVA ---

                // 1. De Request a Entidad (Crear Reserva)
                CreateMap<ReservaRequestDto, ReservaEntity>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.Cod, opt => opt.Ignore()) // Lo generás vos (ej: RES-XJ82)
                    .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => DateTime.Now))
                    // Los AsientosIds los procesarás en el Service para llenar la tabla intermedia
                    .ForMember(dest => dest.ReservaAsientos, opt => opt.Ignore());

                // 2. De Entidad a Response (El Ticket Final)
                CreateMap<ReservaEntity, ReservaResponseDto>()
                    // Navegamos: Reserva -> Funcion -> Pelicula -> Titulo
                    .ForMember(dest => dest.PeliculaTitulo, opt => opt.MapFrom(src => src.Funcion.Pelicula.Titulo))

                    // Navegamos: Reserva -> Funcion -> Sala -> Nombre
                    .ForMember(dest => dest.SalaNombre, opt => opt.MapFrom(src => src.Funcion.Sala.Nombre))

                    // Traemos la fecha de la función
                    .ForMember(dest => dest.FechaHoraFuncion, opt => opt.MapFrom(src => src.Funcion.FechaHora))

                    // Armamos la lista de strings amigables: "Fila A - Asiento 5"
                    .ForMember(dest => dest.AsientosReservados, opt => opt.MapFrom(src =>
                        src.ReservaAsientos.Select(ra => $"Fila {ra.Asiento.Fila} - Asiento {ra.Asiento.Numero}").ToList()))

                    .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.ToString()))

                    // Calculamos el precio total: Cantidad de asientos * Precio de la función
                    .ForMember(dest => dest.PrecioTotal, opt => opt.MapFrom(src =>
                        src.ReservaAsientos.Count * src.Funcion.Precio));



                // --- BLOQUE FUNCIÓN ---

                // 1. De Request a Entidad (Para crear funciones nuevas)
                CreateMap<FuncionResquestDto, FuncionEntity>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.Pelicula, opt => opt.Ignore())
                    .ForMember(dest => dest.Sala, opt => opt.Ignore())
                    .ForMember(dest => dest.Reservas, opt => opt.Ignore())
                    .ForMember(dest => dest.ReservaAsientos, opt => opt.Ignore());

                // 2. De Entidad a Lista (Para la cartelera general)
                CreateMap<FuncionEntity, FuncionResponseListaDto>()
                    .ForMember(dest => dest.PeliculaTitulo, opt => opt.MapFrom(src => src.Pelicula.Titulo))
                    .ForMember(dest => dest.SalaNombre, opt => opt.MapFrom(src => src.Sala.Nombre));

                // 3. De Entidad a Detalle (Para la selección de asientos)
                CreateMap<FuncionEntity, FuncionResponseDetalleDto>()
                    // Datos de Película
                    .ForMember(dest => dest.PeliculaTitulo, opt => opt.MapFrom(src => src.Pelicula.Titulo))
                    .ForMember(dest => dest.PeliculaSinopsis, opt => opt.MapFrom(src => src.Pelicula.Sinopsis))
                    .ForMember(dest => dest.PeliculaDuracion, opt => opt.MapFrom(src => src.Pelicula.Duracion))

                    // Datos de Sala - ¡USANDO LAS PROPIEDADES DIRECTAS! 
                    // Mucho más rápido y sin errores de conversión.
                    .ForMember(dest => dest.SalaNombre, opt => opt.MapFrom(src => src.Sala.Nombre))
                    .ForMember(dest => dest.CantidadFilas, opt => opt.MapFrom(src => src.Sala.CantidadFilas))
                    .ForMember(dest => dest.CantidadColumnas, opt => opt.MapFrom(src => src.Sala.CantidadColumnas))

                    // Mapeamos los IDs de asientos ya ocupados
                    .ForMember(dest => dest.AsientosOcupadosIds, opt => opt.MapFrom(src =>
                        src.ReservaAsientos.Select(ra => ra.AsientoId).ToList()));
            }
        }
    }
