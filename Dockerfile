FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App

# Set environment
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ConnectionStrings__DatabaseConnection=Server=localhost;Database=tattoodb;User=sa;Password=ArtTattoo@@;Trusted_Connection=true;TrustServerCertificate=true;

EXPOSE 8080

COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "art-tattoo-be.dll"]