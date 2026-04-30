# --- ETAPA 1: COMPILACIÓN ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Como ya estamos dentro de la carpeta ApiCine por la configuración de Render,
# el archivo .csproj está en la raíz actual.
COPY ["ApiCine.csproj", "./"]
RUN dotnet restore "ApiCine.csproj"

COPY . .
# No necesitamos hacer WORKDIR /src/ApiCine porque ya estamos en la carpeta del código
RUN dotnet publish "ApiCine.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- ETAPA 2: EJECUCIÓN ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

# CORRECCIÓN AQUÍ: Quitamos "ApiCine/" del origen
RUN mkdir -p Data/Scripts
COPY ["Data/Scripts/SeedData.sql", "Data/Scripts/"]

ENTRYPOINT ["dotnet", "ApiCine.dll"]