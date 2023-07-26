#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FeedMe.Crawler/FeedMe.Crawler.csproj", "FeedMe.Crawler/"]
COPY ["FeedMe.EF/FeedMe.EF.csproj", "FeedMe.EF/"]
COPY ["Ramsey.Core/Ramsey.Core.csproj", "Ramsey.Core/"]
COPY ["FeedMe.Shared/FeedMe.Shared.csproj", "FeedMe.Shared/"]
RUN dotnet restore "FeedMe.Crawler/FeedMe.Crawler.csproj"
COPY . .
WORKDIR "/src/FeedMe.Crawler"
RUN dotnet build "FeedMe.Crawler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FeedMe.Crawler.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FeedMe.Crawler.dll"]