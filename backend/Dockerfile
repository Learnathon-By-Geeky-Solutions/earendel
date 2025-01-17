FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY backend.csproj ./
RUN dotnet restore

COPY backend/ ./backend
COPY appsettings.json ./appsettings.json
COPY appsettings.Development.json ./appsettings.Development.json
COPY Program.cs ./Program.cs

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

RUN adduser --disabled-password --gecos "" appuser \
    && chown -R appuser:appuser /App

USER appuser

COPY --from=build-env /App/out .

EXPOSE 80

ENTRYPOINT ["dotnet", "backend.dll"]
