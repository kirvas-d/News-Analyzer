FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8082
EXPOSE 8083

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/NlpService/NlpService.WebApi/NlpService.WebApi.csproj", "src/NlpService/NlpService.WebApi/"]
COPY ["src/NlpService/NlpService.Data/NlpService.Data.csproj", "src/NlpService/NlpService.Data/"]
COPY ["src/Common/NewsAnalyzer.EfCoreRepository/NewsAnalyzer.EfCoreRepository.csproj", "src/Common/NewsAnalyzer.EfCoreRepository/"]
COPY ["src/Common/NewsAnalyzer.Repository/NewsAnalyzer.Repository.csproj", "src/Common/NewsAnalyzer.Repository/"]
COPY ["src/NlpService/NlpService.Core/NlpService.Core.csproj", "src/NlpService/NlpService.Core/"]
COPY ["src/NewsService/NewsService.Core/NewsService.Core.csproj", "src/NewsService/NewsService.Core/"]
COPY ["src/NlpService/NlpService.NerService/NlpService.NerService.csproj", "src/NlpService/NlpService.NerService/"]
COPY ["src/NlpService/NlpService.SentimentAnalyzeService/NlpService.SentimentAnalyzeService.csproj", "src/NlpService/NlpService.SentimentAnalyzeService/"]
RUN dotnet restore "./src/NlpService/NlpService.WebApi/NlpService.WebApi.csproj"
COPY . .
WORKDIR "/src/src/NlpService/NlpService.WebApi"
RUN dotnet build "./NlpService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NlpService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NlpService.WebApi.dll"]