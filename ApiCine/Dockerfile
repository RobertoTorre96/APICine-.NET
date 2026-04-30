# --- ETAPA 1: COMPILACIÓN ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Usamos un wildcard para encontrar el csproj esté donde esté
COPY **/*.csproj ./
RUN dotnet restore

# Copiamos todo
COPY . .

# Publicamos
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# --- ETAPA 2: EJECUCIÓN ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

# ⚠️ LA SOLUCIÓN AL ERROR DE RUTA:
# En lugar de buscar en carpetas específicas, vamos a buscar el archivo SQL 
# en todo el proyecto y lo movemos a donde la App lo espera.
RUN mkdir -p Data/Scripts
COPY **/SeedData.sql Data/Scripts/

ENTRYPOINT ["dotnet", "ApiCine.dll"]