var AWS = require("aws-sdk");

AWS.config.update({
  region: "us-west-2",
  endpoint: "http://localhost:8000"
});

var dynamodb = new AWS.DynamoDB();

// Table variables
const tableName = "Customers";
const customerId = "customerId";
const customerName = "customerName";
const indexCustomerName = "indexCustomerName";
//const customerEmail = "customerEmail";

var params = {
    TableName : tableName,
    // Set the primary key of the table (can be one part PK or a two part)
    KeySchema: [       
        { AttributeName: customerId, KeyType: "HASH"},  // One part primary key
    ],
    // Additional attributes for the item
    AttributeDefinitions: [       
        { AttributeName: customerId, AttributeType: "S" },
        { AttributeName: customerName, AttributeType: "S" },
       // { AttributeName: customerEmail, AttributeType: "S" }
    ],
    // Set up a secondary index for table. Used to query the 'cutomerName'
    GlobalSecondaryIndexes: [{
        IndexName: indexCustomerName,
        KeySchema: [
            {
                AttributeName: customerName,
                KeyType: "HASH"
            },
           /*  {
                AttributeName: customerEmail,
                KeyType: "RANGE"
            } */
        ],
        Projection: {
            ProjectionType: "ALL"
        },
        ProvisionedThroughput: {
            ReadCapacityUnits: 1,
            WriteCapacityUnits: 1
        }
    }],
    ProvisionedThroughput: {    // Ignored with downloadable dynamodb
        ReadCapacityUnits: 1, 
        WriteCapacityUnits: 1
    }
};

dynamodb.createTable(params, function(err, data) {
    if (err) {
        console.error("Unable to create table. Error JSON:", JSON.stringify(err, null, 2));
    } else {
        console.log("Created table. Table description JSON:", JSON.stringify(data, null, 2));
    }
});