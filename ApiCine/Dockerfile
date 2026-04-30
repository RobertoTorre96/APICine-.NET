FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos el proyecto usando la ruta que confirmamos en tu GitHub
COPY ["ApiCine/ApiCine.csproj", "ApiCine/"]
RUN dotnet restore "ApiCine/ApiCine.csproj"

# Copiamos todo el contenido del repo
COPY . .

# Publicamos el proyecto
WORKDIR "/src/ApiCine"
RUN dotnet publish "ApiCine.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- ETAPA DE EJECUCIÓN ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Aquí es donde fallaba: ahora 'build' está definido arriba
COPY --from=build /app/publish .

# Copiamos el script de datos
RUN mkdir -p Data/Scripts
COPY ["ApiCine/Data/Scripts/SeedData.sql", "Data/Scripts/"]

ENTRYPOINT ["dotnet", "ApiCine.dll"]