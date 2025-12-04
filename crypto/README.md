# Sample Hardhat 3 Beta Project (minimal)

This project has a minimal setup of Hardhat 3 Beta, without any plugins.

## What's included?

The project includes native support for TypeScript, Hardhat scripts, tasks, and support for Solidity compilation and tests.

ID kontraktu: 0x76a7854fB1f3bAd944F7aD88D5b9f749430f5613
ABI tokenu w `abi.json`
Endpoint i klucz admina w `.env`

## Deploying to Local Anvil (Docker)

The smart contract is automatically deployed to a local Anvil node when running the full Docker stack:

```bash
docker compose up -d
```

This will:
1. Start Anvil (local Ethereum node) on `localhost:8545`
2. Build and run the crypto service which:
   - Waits for Anvil to be ready
   - Compiles the SensorToken contract
   - Deploys it using Hardhat
   - Outputs the deployed contract address

Monitor the deployment logs with:
```bash
docker compose logs -f crypto
```

### Local Network Configuration

The Hardhat config includes a `localhost` network pointing to the Anvil container at `http://anvil:8545`.

## Generating Wallets
You can generate wallets using the following command:

```bash
npx hardhat run scripts/create-wallet.ts
```
