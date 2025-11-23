import hre from "hardhat";

async function main() {
  const [deployer] = await hre.ethers.getSigners();
  console.log("Deploying contracts with account:", deployer.address);

  // Tworzymy fabrykę kontraktu
  const TokenFactory = await hre.ethers.getContractFactory("SensorToken");

  // Deploy kontraktu
  const token = await TokenFactory.deploy();

  // W ethers v6 czekamy na deploy tak:
  await token.waitForDeployment();

  console.log("SensorToken deployed to:", await token.getAddress());
}

main().catch((error) => {
  console.error(error);
  process.exit(1);
});
