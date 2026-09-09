FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first for better restore caching
COPY ["rsm_backend.sln", "./"]

COPY ["rsm_backend.Api/rsm_backend.Api.csproj", "rsm_backend.Api/"]
COPY ["rsm_backend.Application/rsm_backend.Application.csproj", "rsm_backend.Application/"]
COPY ["rsm_backend.Domain/rsm_backend.Domain.csproj", "rsm_backend.Domain/"]
COPY ["rsm_backend.Infrastructure/rsm_backend.Infrastructure.csproj", "rsm_backend.Infrastructure/"]

RUN dotnet restore "rsm_backend.Api/rsm_backend.Api.csproj"

# Copy the rest of the source code
COPY . .

WORKDIR "/src/rsm_backend.Api"

RUN dotnet publish "rsm_backend.Api.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "rsm_backend.Api.dll"]