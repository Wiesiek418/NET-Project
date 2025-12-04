import hre from "hardhat";
import fs from "fs";

async function main() {
  const [deployer] = await hre.ethers.getSigners();
  console.log("Deploying contracts with account:", deployer.address);

  // Tworzymy fabrykę kontraktu
  const TokenFactory = await hre.ethers.getContractFactory("SensorToken");

  // Deploy kontraktu
  const token = await TokenFactory.deploy();

  // W ethers v6 czekamy na deploy tak:
  await token.waitForDeployment();

  const address = await token.getAddress();
  console.log("SensorToken deployed to:", address);

  fs.writeFileSync("/deploy/token.json", JSON.stringify({
    tokenAddress: address
  }, null, 2));
}

main().catch((error) => {
  console.error(error);
  process.exit(1);
});
