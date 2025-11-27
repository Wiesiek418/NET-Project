// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

import "@openzeppelin/contracts/token/ERC20/ERC20.sol";
import "@openzeppelin/contracts/access/Ownable.sol";

/**
 * @title SensorToken
 * @dev ERC20 token used to reward IoT sensors for sending data.
 */
contract SensorToken is ERC20, Ownable {
    uint256 public constant INITIAL_SUPPLY = 1_000_000 * 10**18;

    constructor() ERC20("SensorToken", "ST") Ownable(msg.sender) {
        _mint(msg.sender, INITIAL_SUPPLY);
    }

    /**
     * @notice Rewards a sensor by transferring tokens to its wallet address.
     * @param sensor The sensor's blockchain wallet address.
     * @param amount Number of tokens to reward (in wei: * 10^18).
     *
     * Requirements:
     * - only owner (backend/C# system) can call this
     */
    function rewardSensor(address sensor, uint256 amount) external onlyOwner {
        require(sensor != address(0), "Invalid sensor address");
        require(balanceOf(msg.sender) >= amount, "Insufficient tokens");

        _transfer(msg.sender, sensor, amount);
    }
}
