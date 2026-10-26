
# ============================================================
# PortfolioWork - UserService 
# ============================================================

# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY src/UserService/UserService.csproj src/UserService/
RUN dotnet restore src/UserService/UserService.csproj

# Copy source files and publish
COPY src/UserService/ src/UserService/
RUN dotnet publish src/UserService/UserService.csproj -c Release -o /app/publish --no-restore

# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
# Install curl for Docker health checks.
# --no-install-recommends avoids unnecessary packages.
# Remove package lists afterward to reduce image size.
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "UserService.dll"]
