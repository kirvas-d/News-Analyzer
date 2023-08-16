FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Application/NewsAnalyzer.Application.NewsService/NewsAnalyzer.Application.NewsService.csproj", "src/Application/NewsAnalyzer.Application.NewsService/"]
COPY ["src/Core/NewsAnalyzer.Core/NewsAnalyzer.Core.csproj", "src/Core/NewsAnalyzer.Core/"]
COPY ["src/Infrastructure/NewsAnalyzer.Infrastructure.EfCoreRepository/NewsAnalyzer.Infrastructure.EfCoreRepository.csproj", "src/Infrastructure/NewsAnalyzer.Infrastructure.EfCoreRepository/"]
COPY ["src/Infrastructure/NewsAnalyzer.Infrastructure.RabbitMqService/NewsAnalyzer.Infrastructure.RabbitMqService.csproj", "src/Infrastructure/NewsAnalyzer.Infrastructure.RabbitMqService/"]
RUN dotnet restore "src/Application/NewsAnalyzer.Application.NewsService/NewsAnalyzer.Application.NewsService.csproj"
COPY . .
WORKDIR "/src/src/Application/NewsAnalyzer.Application.NewsService"
RUN dotnet build "NewsAnalyzer.Application.NewsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NewsAnalyzer.Application.NewsService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN pwsh playwright.ps1 install
RUN pwsh playwright.ps1 install-deps
ENTRYPOINT ["dotnet", "NewsAnalyzer.Application.NewsService.dll"]