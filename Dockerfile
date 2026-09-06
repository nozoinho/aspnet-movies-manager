# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY ["MovieCatalog.csproj", "./"]
RUN dotnet restore "MovieCatalog.csproj"

COPY . .

RUN dotnet publish "MovieCatalog.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["sh", "-c", "dotnet MovieCatalog.dll --urls http://0.0.0.0:${PORT:-8080}"]