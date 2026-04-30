# --- ETAPA 2: EJECUCIÓN (Runtime de .NET 8) ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Puerto que usa Render por defecto
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Copiar los archivos publicados desde la etapa de compilación
COPY --from=build /app/publish .

# 4. CORRECCIÓN DE RUTA PARA EL SCRIPT SQL
# Creamos la carpeta dentro del contenedor
RUN mkdir -p Data/Scripts

# Copiamos el archivo desde la carpeta del proyecto (donde vive en GitHub)
# hacia la carpeta que acabamos de crear en el contenedor.
COPY ["ApiCine/Data/Scripts/SeedData.sql", "Data/Scripts/"]

# Comando para arrancar la API
ENTRYPOINT ["dotnet", "ApiCine.dll"]