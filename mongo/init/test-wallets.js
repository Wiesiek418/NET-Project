const appDb = db.getSiblingDB("myapp");

appDb.createCollection("wallets");

// Insert test wallets
appDb.sensor_wallets.insertMany([
    { 
        id: "1", 
        address: "0xe3587046989c92D55bA245E6DC941EE47C479a92", 
        type: "Lorem Ipsum" 
    },
]);

print("Wallets test data inserted.");
