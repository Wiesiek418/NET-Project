# NET-Project

## API

### Blockchain

#### Wallets

All known wallets
```http
GET /api/wallet
```
results in
```json
[
    {
        "SensorType":"SensorA",
        "Address":"0x123",
        "Id":"1"
    }
]
```
---

Balance for all wallets
```http
GET api/Wallet/balances
```
results in
```json
[
    {
        "SensorType":"SensorA",
        "Balance":1000000,
        "Id":"1"
    }
]
```
> Test data in: \api\api_src\Domains\Blockchain\Application\WalletService.cs:50

### WebSocket Notifications
SignalR WebSocket endpoint
```
/api/notifications
```

## Development

### Backend
Get .NET 9 SDK from [here](https://dotnet.microsoft.com/en-us/download).  
You already have Docker, right ?

## Testing
For MQTT sensor simulations, see the `sensor` directory.


## Deployment

### Locally
Local Ethereum node
```bash
docker compose -f docker-compose.local.yml up -d
```

### Server
Sepolia testnet through Infura
```bash
docker compose -f docker-compose.sepolia.yml up -d
```