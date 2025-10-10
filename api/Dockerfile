FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY api_src/*.csproj ./
RUN dotnet restore

COPY api_src/. ./
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .

EXPOSE 5000
ENTRYPOINT ["dotnet", "api_src.dll"]
