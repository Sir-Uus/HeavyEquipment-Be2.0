# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy csproj files and restore as distinct layers
COPY *.sln ./
COPY API/API.csproj API/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Persistence/Persistence.csproj Persistence/

RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

RUN mkdir -p /app/certificates
COPY API/Certificate/aspnetapp.pfx /app/certificates

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS="http://+:80;https://+:443"

ENTRYPOINT [ "dotnet", "API.dll" ]
