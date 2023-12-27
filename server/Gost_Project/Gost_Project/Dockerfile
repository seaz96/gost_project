FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Gost_Project.csproj", "Gost_Project/"]
RUN dotnet restore "Gost_Project.csproj"
COPY . .
WORKDIR "/src/Gost_Project"
RUN dotnet build "Gost_Project.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gost_Project.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gost_Project.dll"]