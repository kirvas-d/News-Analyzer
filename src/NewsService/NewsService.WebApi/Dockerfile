#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/NewsService/NewsService.WebApi/NewsService.WebApi.csproj", "src/NewsService/NewsService.WebApi/"]
COPY ["src/NewsService/NewsService.Core/NewsService.Core.csproj", "src/NewsService/NewsService.Core/"]
COPY ["src/Common/NewsAnalyzer.Repository/NewsAnalyzer.Repository.csproj", "src/Common/NewsAnalyzer.Repository/"]
COPY ["src/NewsService/NewsService.Repository/NewsService.Repository.csproj", "src/NewsService/NewsService.Repository/"]
COPY ["src/Common/NewsAnalyzer.EfCoreRepository/NewsAnalyzer.EfCoreRepository.csproj", "src/Common/NewsAnalyzer.EfCoreRepository/"]
RUN dotnet restore "./src/NewsService/NewsService.WebApi/NewsService.WebApi.csproj"
COPY . .
WORKDIR "/src/src/NewsService/NewsService.WebApi"
RUN dotnet build "./NewsService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NewsService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NewsService.WebApi.dll"]