#!/bin/sh
set -e

# echo "Waiting for anvil to be ready..."
# MAX_ATTEMPTS=60
# ATTEMPT=0

# while [ $ATTEMPT -lt $MAX_ATTEMPTS ]; do
#   if timeout 2 bash -c "echo > /dev/tcp/anvil/8545" 2>/dev/null; then
#     echo "Anvil is ready!"
#     break
#   fi
#   ATTEMPT=$((ATTEMPT + 1))
#   echo "Anvil not ready yet (attempt $ATTEMPT/$MAX_ATTEMPTS)..."
#   sleep 1
# done

# if [ $ATTEMPT -eq $MAX_ATTEMPTS ]; then
#   echo "ERROR: Anvil failed to respond within timeout"
#   exit 1
# fi

echo "Deploying smart contract..."
npx hardhat run scripts/deploy.ts --network localhost

echo "Deployment complete!"
sleep infinity