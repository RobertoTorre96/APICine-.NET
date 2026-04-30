FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. El .csproj está en la misma carpeta que el Dockerfile
COPY ["ApiCine.csproj", "./"]
RUN dotnet restore "ApiCine.csproj"

# 2. Copiamos todo el contenido de la carpeta ApiCine
COPY . .
RUN dotnet build "ApiCine.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiCine.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# 3. RUTA CORREGIDA: 
# Como el Dockerfile está en 'ApiCine', 'Data' es una carpeta hermana.
COPY Data/Scripts/SeedData.sql ./Data/Scripts/SeedData.sql

ENTRYPOINT ["dotnet", "ApiCine.dll"]