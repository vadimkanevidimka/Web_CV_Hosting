#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["/AuthService.Presentation/AuthService.Presentation.csproj", "AuthService.Presentation/"]
COPY ["/AuthService.Buisness/AuthService.Buisness.csproj", "AuthService.Buisness/"]
COPY ["/AuthService.DataAccess/AuthService.DataAccess.csproj", "AuthService.DataAccess/"]
RUN dotnet restore "./AuthService.Presentation/./AuthService.Presentation.csproj"
COPY . .
WORKDIR "/src/AuthService.Presentation"
RUN dotnet build "./AuthService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./AuthService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.Presentation.dll"]