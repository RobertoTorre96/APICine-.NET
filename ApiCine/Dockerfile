# --- ETAPA 1: COMPILACIÓN (SDK de .NET 8) ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiar el archivo del proyecto y restaurar las dependencias (NuGet)
# Esto se hace primero para que Docker cachee las librerías y sea más rápido
COPY ["ApiCine.csproj", "./"]
RUN dotnet restore "ApiCine.csproj"

# 2. Copiar el resto del código fuente del proyecto
COPY . .

# 3. Compilar y publicar la aplicación en la carpeta /app/publish
# Usamos el modo Release para que el código esté optimizado
RUN dotnet publish "ApiCine.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- ETAPA 2: EJECUCIÓN (Runtime de .NET 8) ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Puerto que usa Render por defecto
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Copiar los archivos publicados desde la etapa de compilación
COPY --from=build /app/publish .

# 4. ¡CRUCIAL PARA TU CV! Copiar los scripts de datos manualmente.
# Esto asegura que el Seeder encuentre el archivo .sql en el servidor de Render
# Ajusta "Data/Scripts/" si el nombre de tu carpeta es diferente (respeta Mayúsculas)
COPY ["Data/Scripts/SeedData.sql", "Data/Scripts/"]

# Comando para arrancar la API
# Asegúrate de que el nombre coincida con tu archivo .dll generado
ENTRYPOINT ["dotnet", "ApiCine.dll"]