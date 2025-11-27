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

# Set default environment variables that can be overridden by docker-compose or runtime
ENV ASPNETCORE_ENVIRONMENT=Production
# Shared Database Configuration
ENV Database__ConnectionString=mongodb://root:example@mongodb:27017
ENV Database__DatabaseName=sensordb
# Sensors Domain Configuration
ENV Sensors__Collections__Conveyor=conveyor_readings
ENV Sensors__Collections__Baking=baking_readings
ENV Sensors__Collections__Dough=dough_readings
ENV Sensors__Collections__Packing=packing_readings
ENV Sensors__Mqtt__BrokerHost=mqtt
ENV Sensors__Mqtt__BrokerPort=1883
ENV Sensors__Mqtt__ClientId=mqtt-listener
ENV Sensors__Mqtt__Username=
ENV Sensors__Mqtt__Password=
ENV Sensors__Mqtt__Topics__Conveyor=sensors/conveyor
ENV Sensors__Mqtt__Topics__Baking=sensors/baking
ENV Sensors__Mqtt__Topics__Dough=sensors/dough
ENV Sensors__Mqtt__Topics__Packing=sensors/packing
# Blockchain Domain Configuration
ENV Blockchain__Collections__Blockchain=Blockchains
ENV Blockchain__RpcUrl=https://sepolia.infura.io/v3/3a9054388e6748308aac294ff334b4c2
ENV Blockchain__TokenAddress=0x76a7854fB1f3bAd944F7aD88D5b9f749430f5613
ENV Blockchain__TokenDecimals=18

ENTRYPOINT ["dotnet", "api_src.dll"]
