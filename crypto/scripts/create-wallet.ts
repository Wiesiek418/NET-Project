import { ethers } from "ethers";

function createWallets(num: number) {
    const wallets = [];
    for (let i = 1; i <= num; i++) {
        const wallet = ethers.Wallet.createRandom();
        wallets.push({
            walletNumber: i,
            address: wallet.address,
            privateKey: wallet.privateKey,
        });
    }
    console.log(JSON.stringify(wallets, null, 2));
}

createWallets(16);
