# --- ETAPA 1: COMPILACIÓN ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiamos el archivo de proyecto especificando la carpeta
# La ruta en el origen es ApiCine/ApiCine.csproj
COPY ["ApiCine/ApiCine.csproj", "ApiCine/"]

# Restauramos dependencias apuntando al archivo en su nueva ubicación
RUN dotnet restore "ApiCine/ApiCine.csproj"

# 2. Copiamos todo el contenido (esto incluye la carpeta ApiCine y su código)
COPY . .

# 3. Publicamos apuntando al archivo de proyecto correcto
WORKDIR "/src/ApiCine"
RUN dotnet publish "ApiCine.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- ETAPA 2: EJECUCIÓN ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

# 4. Copiamos el script SQL (Verificá que el nombre de carpeta coincida)
# Como ya estamos dentro de /app después del publish, recreamos la ruta
RUN mkdir -p Data/Scripts
COPY ["Data/Scripts/SeedData.sql", "Data/Scripts/"]

ENTRYPOINT ["dotnet", "ApiCine.dll"]