#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Ramsey.NET/Ramsey.NET.csproj", "Ramsey.NET/"]
COPY ["FeedMe.EF/FeedMe.EF.csproj", "FeedMe.EF/"]
COPY ["Ramsey.Core/Ramsey.Core.csproj", "Ramsey.Core/"]
COPY ["FeedMe.Shared/FeedMe.Shared.csproj", "FeedMe.Shared/"]
RUN dotnet restore "Ramsey.NET/Ramsey.NET.csproj"
COPY . .
WORKDIR "/src/Ramsey.NET"
RUN dotnet build "Ramsey.NET.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ramsey.NET.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ramsey.NET.dll"]