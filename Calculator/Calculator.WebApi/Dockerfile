FROM mcr.microsoft.com/dotnet/sdk:5.0 AS builder

WORKDIR /app

# Copy the main source project files
COPY *.csproj ./

# Restore dependencies
RUN dotnet restore

# Copy everything else and build
COPY . ./

# Publish application in Release
WORKDIR /app

RUN dotnet publish -c Release -o dist

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime

WORKDIR /app

COPY --from=builder /app/dist .

ENTRYPOINT ["dotnet", "Calculator.WebApi.dll"]