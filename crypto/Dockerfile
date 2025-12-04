# # Build stage
# FROM node:22-alpine AS builder

# WORKDIR /app

# # Copy package files
# COPY package*.json ./
# COPY tsconfig.json ./

# # Install dependencies
# RUN npm install
# RUN npm ci

# # Copy source files
# COPY contracts ./contracts
# COPY scripts ./scripts
# COPY hardhat.config.ts ./

# # Build stage complete


# # Runtime stage
# FROM node:22-alpine

# WORKDIR /app

# # Install curl for healthchecks
# RUN apk add --no-cache curl

# # Copy built dependencies and source from builder
# COPY --from=builder /app/node_modules ./node_modules
# COPY --from=builder /app/package*.json ./
# COPY --from=builder /app/tsconfig.json ./
# COPY contracts ./contracts
# COPY scripts ./scripts
# COPY hardhat.config.ts ./

# # Install netcat for waiting for anvil
# RUN apk add --no-cache netcat-openbsd

# # Create a deployment script that waits for anvil
# COPY entrypoint.sh entrypoint.sh

# RUN chmod +x ./entrypoint.sh

# ENTRYPOINT ["./entrypoint.sh"]

FROM node:22

WORKDIR /app
COPY package*.json .
RUN npm install
COPY . .

ENTRYPOINT ["npm", "run", "deploy"]

