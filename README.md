# NET-Project

## API

### Blockchain

#### Wallets

All known wallets
```http
GET GET /api/wallet
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


## Development

### Backend
Get .NET 9 SDK from [here](https://dotnet.microsoft.com/en-us/download).  
You already have Docker, right ?

## Testing
For MQTT sensor simulations, see the `sensor` directory.
