FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BlogpostService.csproj", "BlogpostService/"]
RUN dotnet restore "BlogpostService/BlogpostService.csproj"
COPY . .
WORKDIR "/src/BlogpostService"
RUN dotnet build "./BlogpostService.csproj" -c $BUILD_CONFIGURATION -o /app/build -no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BlogpostService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogpostService.dll"]
