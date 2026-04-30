FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar
COPY ["ApiCine.csproj", "./"]
RUN dotnet restore "./ApiCine.csproj"

# Copiar todo el código y compilar
COPY . .
RUN dotnet build "ApiCine.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiCine.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# --- ESTA LÍNEA ES CRUCIAL ---
# Copiamos el SQL explícitamente a la imagen final
COPY Data/Scripts/SeedData.sql ./Data/Scripts/SeedData.sql

ENTRYPOINT ["dotnet", "ApiCine.dll"]