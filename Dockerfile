FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["BlogpostService/BlogpostService.csproj", "BlogpostService/"]
RUN dotnet restore "BlogpostService/BlogpostService.csproj"

COPY . .
WORKDIR "/src/BlogpostService"

RUN dotnet build "BlogpostService.csproj" -c Release -o /app/build
RUN dotnet publish "BlogpostService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BlogpostService.dll"]