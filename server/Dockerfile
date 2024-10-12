FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GostStorage/GostStorage.csproj", "GostStorage/"]
RUN dotnet restore "GostStorage/GostStorage.csproj"
COPY . .
WORKDIR "/src/GostStorage"
RUN dotnet build "GostStorage.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GostStorage.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GostStorage.dll"]