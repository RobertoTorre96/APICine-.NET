FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiar el archivo de proyecto (está en la misma carpeta que el Dockerfile)
COPY ApiCine.csproj ./
RUN dotnet restore "ApiCine.csproj"

# 2. Copiar todo el contenido de la carpeta actual
COPY . .
RUN dotnet build "ApiCine.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiCine.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiamos la aplicación publicada
COPY --from=publish /app/publish .

# 3. RUTA DEL SCRIPT SQL:
# Usamos el asterisco para que Docker lo encuentre sin importar 
# si la ruta tiene diferencias de mayúsculas/minúsculas en el servidor
# Esto busca SeedData.sql donde sea que esté y lo pone en el destino correcto
COPY **/SeedData.sql ./Data/Scripts/SeedData.sql
ENTRYPOINT ["dotnet", "ApiCine.dll"]