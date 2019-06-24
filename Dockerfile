FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine3.9 AS build-env
WORKDIR /app

RUN apk --update add yarn nodejs

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine3.9
WORKDIR /app
COPY --from=build-env /app/Ramsey.NET/out .
CMD [ "dotnet","Ramsey.NET.dll" ]