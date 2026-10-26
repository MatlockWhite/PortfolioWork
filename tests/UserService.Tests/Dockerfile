
# ============================================================
# PortfolioWork - UserService Integration Tests
# ============================================================

# The .NET SDK is required to build and execute xUnit tests.
FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

# Copy the test project and restore NuGet dependencies.
COPY tests/UserService.Tests/UserService.Tests.csproj tests/UserService.Tests/
RUN dotnet restore tests/UserService.Tests/UserService.Tests.csproj

# Copy the test source code.
COPY tests/UserService.Tests/ tests/UserService.Tests/

# Execute tests when the container starts.
ENTRYPOINT ["dotnet", "test", "tests/UserService.Tests/UserService.Tests.csproj", "--no-restore", "--logger", "console;verbosity=normal"]
