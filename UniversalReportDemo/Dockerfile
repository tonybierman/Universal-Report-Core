# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy and restore the project dependencies
COPY ["UniversalReportDemo/UniversalReportDemo.csproj", "UniversalReportDemo/"]
RUN dotnet restore "./UniversalReportDemo/UniversalReportDemo.csproj"

# Copy entire project and build
COPY . .
WORKDIR "/src/UniversalReportDemo"
RUN dotnet build "./UniversalReportDemo.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the service project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UniversalReportDemo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/UniversalReportDemo/wwwroot /app/wwwroot 
ENTRYPOINT ["dotnet", "UniversalReportDemo.dll"]
