# 1. ETAPA DE CONSTRUCCIÓN (BUILD)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos el proyecto y restauramos dependencias
# Usamos comodín por si hay variaciones en el nombre
COPY *.csproj ./
RUN dotnet restore

# Copiamos todo el código fuente
COPY . .

# Compilamos la aplicación
RUN dotnet build "ApiCine.csproj" -c Release -o /app/build

# 2. ETAPA DE PUBLICACIÓN (PUBLISH)
FROM build AS publish
RUN dotnet publish "ApiCine.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 3. ETAPA FINAL (EJECUCIÓN)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiamos los archivos publicados desde la etapa anterior
COPY --from=publish /app/publish .

# --- EL CAMBIO CLAVE ---
# Creamos la carpeta de destino y copiamos el SQL explícitamente
# Esto asegura que el archivo esté en /app/Data/Scripts/SeedData.sql
COPY Data/Scripts/SeedData.sql ./Data/Scripts/SeedData.sql

# Comando para iniciar la API
ENTRYPOINT ["dotnet", "ApiCine.dll"]