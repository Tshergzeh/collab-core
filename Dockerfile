FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/CollabCore.Api/CollabCore.Api.csproj src/CollabCore.Api/
COPY src/CollabCore.Core/CollabCore.Core.csproj src/CollabCore.Core/
COPY src/CollabCore.Infrastructure/CollabCore.Infrastructure.csproj src/CollabCore.Infrastructure/
COPY src/CollabCore.Contracts/CollabCore.Contracts.csproj src/CollabCore.Infrastructure/
RUN dotnet restore "src/CollabCore.Api/CollabCore.Api/csproj"

COPY . .
WORKDIR /src/src/CollabCore.Api
RUN dotnet build "CollabCore.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet build "CollabCore.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "CollabCore.Api.dll" ]
