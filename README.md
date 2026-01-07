# NET-Project

Project created as part of univeristy course on blockchain technologies.
Includes services for the API, a web UI, Ethereum tooling, MQTT-based sensor
simulations, and local infrastructure orchestration with Docker Compose.

**Contents**
- `anvil/`: Local Ethereum node image and entrypoint used for local deployment.
- `api/`: .NET API service (source, config, and domain logic).
- `crypto/`: Smart contracts, deployment scripts, and Hardhat config.
- `mongo/`: MongoDB data directory used by the local compose setup.
- `mqtt/`: Mosquitto configuration and persisted data.
- `sensor/`: Python MQTT sensor simulator.
- `web/`: Frontend (Vite + Vue) with dashboard and dynamic tabular UI.

**Key goals**
- Demonstrate multi-layer integrated system - frontend, backend, database, async communication with MQTT.
- Provide an API with blockchain-aware features (wallets, balances).
- Simulate tokenization or on-chain interactions via local Ethereum node or Sepolia.

**Architecture overview**
- MQTT sensor publishers (in `sensor/`) -> Mosquitto broker (`mqtt/`).
- API service (`api/`) ingests/processes data, persists to MongoDB, and interacts with blockchain tools in `crypto/`.
- Frontend (`web/`) connects to the API and receives SignalR notifications from `/api/notifications`.

Responsibilities
------------------
- @Wiesiek418 - entire frontend app
- @m-szym - backend, database, mqtt, ethereum integration, dev ops
- @Basileus1990 - backend, database, ethereum integration
- @MateuszTk - sensor data generation, mqtt, ethereum integration

Quickstart
----------

Start the local stack:

```bash
docker compose -f docker-compose.local.yml up -d
```

This brings up:
- MongoDB
- Mosquitto MQTT broker
- backend API
- frontend app
- sensor data generator
- local Ethereum node (Anvil)

To use Sepolia (Infura) instead of local Anvil:

```bash
docker compose -f docker-compose.sepolia.yml up -d
```

Modules
------------------

Web UI (`web/`)
- Vite + Vue frontend app.
- Displays sensor reading history in the form of diagrams or tables, supporting sorting and filtering.
- Offers a dynamic dashboard, updated via WebSocket.
- Allows to inspect the blockchain wallets of all the sensors.

Backend (`api/`)
- Implemented in .NET 9.
- Configuration and environment-specific settings are in `api/api_src/appsettings.json` and `api/api_src/Properties/launchSettings.json`.
- Receives sensor readings from MQTT, notifies via WebSocket and records them in MongoDB instance.
- Serves dynamically filtered and sorted readings through REST API.
- Rewards the sensors with custom smart contract transfer to their wallets.
- Queries Ethereum node (local or Infura) for wallet balances.

Crypto & Contracts (`crypto/`)
- Contains simple smart contract based on ERC20 and Hardhat configuration.
- Can be deployed in Docker container.

Sensor simulation (`sensor/`)
- Python scripts simulating various kinds of sensors emitting JSON-encoded readings to MQTT broker.
- The number of sensors, generation interval and randomness can be configured via `sensor/run_all.py`.

