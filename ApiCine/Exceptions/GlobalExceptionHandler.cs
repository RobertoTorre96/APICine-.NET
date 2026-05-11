using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApiCine.Exceptions {
    public class GlobalExceptionHandler : IExceptionHandler {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext
                                             ,Exception exception
                                             ,CancellationToken cancellationToken) {

            _logger.LogError(exception, "Error capturado por el GlobalHandler: {Message}", exception.Message);

            // Si es nuestra excepción personalizada usamos 404, si no 500
            var (statusCode, title) = exception switch {
                NotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
                AlreadyExistsException => (StatusCodes.Status409Conflict, "Conflicto: El recurso ya existe"),
                ValidationException => (StatusCodes.Status400BadRequest, "Error de validación"),
                BadRequestException => (StatusCodes.Status400BadRequest, "Solicitud incorrecta"),
                // Cualquier otro error no controlado (como fallos de base de datos inesperados)
                _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor")            };

            var problemDetails = new ProblemDetails {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;

        }
    }
}
