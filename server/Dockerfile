FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY server.sln ./
COPY ["GostStorage.API/GostStorage.API.csproj", "GostStorage.API/"]
COPY ["GostStorage.Domain/GostStorage.Domain.csproj", "GostStorage.Domain/"]
COPY ["GostStorage.Infrastructure/GostStorage.Infrastructure.csproj", "GostStorage.Infrastructure/"]
COPY ["GostStorage.Services/GostStorage.Services.csproj", "GostStorage.Services/"]

RUN dotnet restore
COPY . .

WORKDIR "/src/GostStorage.Domain"
RUN dotnet build "GostStorage.Domain.csproj" -c Release -o /app/build
WORKDIR "/src/GostStorage.Services"
RUN dotnet build "GostStorage.Services.csproj" -c Release -o /app/build
WORKDIR "/src/GostStorage.Infrastructure"
RUN dotnet build "GostStorage.Infrastructure.csproj" -c Release -o /app/build
WORKDIR "/src/GostStorage.API"
RUN dotnet build "GostStorage.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GostStorage.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GostStorage.API.dll"]