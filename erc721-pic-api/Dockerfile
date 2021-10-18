#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["erc721-pic-api.csproj", "erc721-pic-api/"]
RUN dotnet restore "erc721-pic-api/erc721-pic-api.csproj"
WORKDIR "/src/erc721-pic-api"
COPY . .
RUN dotnet build "erc721-pic-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "erc721-pic-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "erc721-pic-api.dll"]