const Joi = require('joi');
const uuidv1 = require('uuid/v1');
const express = require('express'); //returns a function
const app = express();

// Used for dynamodb communication
var AWS = require("aws-sdk");

AWS.config.update({
    region: "us-west-2",
    endpoint: "http://localhost:8000"
});

//enables json in a request body
app.use(express.json());

// db table Paramters
const tableName = "Customers";
const customerId = "customerId";
const customerName = "customerName";
const indexCustomerName = "indexCustomerName";
//const customerEmail = "customerEmail";

function sendSuccess(res, data) {
    var output = JSON.stringify({ error: null, data: data }) + "\n";
    res.status(200).send(output);
}

function sendFailure(res, err) {
    var output = JSON.stringify({ error: err.code, data: err.message }) + "\n";
    res.status(err.code).send(output);
}

function makeError(err, msg) {
    var e = new Error(msg);
    e.code = err;
    return e;
}



// ------------------------------ GET ------------------------------

// Scan the whole table without a 'filterExpression'
function getAllCustomers(callback) {

    var docClient = new AWS.DynamoDB.DocumentClient();

    var params = {
        TableName: tableName,
        ProjectionExpression: "#name",
        ExpressionAttributeNames: {
            "#name": customerName,
        }
    };

    docClient.scan(params, (err, data) => {
        if (err) {
            // Scan failed
            var serverError = new makeError(500, 'Unable to scan. Error' + JSON.stringify(err, null, 2));
            callback(serverError, null);
            //console.error("Unable to scan the table. Error JSON:", JSON.stringify(err, null, 2));
        } else {
            // print all the customers
            var customers = []
            console.log("Scan succeeded.");
            data.Items.forEach(function (scannedCustomer) {
                customers.push(scannedCustomer);
                console.log("customer name: ", scannedCustomer.customerName);
            });
            // continue scanning if we have more customers, because
            // scan can retrieve a maximum of 1MB of data
            if (typeof data.LastEvaluatedKey != "undefined") {
                console.log("Scanning for more...");
                params.ExclusiveStartKey = data.LastEvaluatedKey;
                docClient.scan(params, onScan);
            }
            callback(null, customers);
        }
    });
}

app.get('/api/customers', (req, res) => {
    getAllCustomers((err, allCustomers) => {
        if (err) {
            sendFailure(res, err);
        }
        else {
            sendSuccess(res, allCustomers);
        }
    });
});


function findCustomer(requestName, callback) {
    var docClient = new AWS.DynamoDB.DocumentClient();

    var params = {
        TableName: tableName,
        IndexName: indexCustomerName,
        KeyConditionExpression: "#name = :reqName",
        ExpressionAttributeNames: {
            "#name": customerName
        },
        ExpressionAttributeValues: {
            ":reqName": requestName
        }
    };

    docClient.query(params, (err, data) => {
        if (err) {
            // Query faild
            var serverError = new makeError(500, 'Unable to query. Error' + JSON.stringify(err, null, 2));
            callback(serverError, null);
        } else {
            if (data.Items.length < 1) {
                // No customer with requested name
                callback(new makeError(404, `No customer by the name: ${requestName}`, null));
            }
            else {
                var retrievedName = data.Items[0].customerName;
                callback(null, retrievedName);
            }
        }
    });

}

app.get('/api/customers/:id', (req, res) => {
    const name = req.params.id.toString();
    console.log('I am looking for customer: ', name);
    if (!name) {
        sendFailure(res, new makeError(400, `Invalid customer name: ${name}.`));
        return;
    }
    findCustomer(name, (err, retrievedCustomer) => {
        if (err) {
            sendFailure(res, err);
        }
        else {
            sendSuccess(res, retrievedCustomer);
        }
    });
});


// ------------------------------ POST ------------------------------

function addCustomer(customer, callback) {

    var docClient = new AWS.DynamoDB.DocumentClient();
    console.log("the new customer info -> \tid: ", customer.id, "\tname: ", customer.name);
    var params = {
        TableName: tableName,
        Item: {
            customerId: customer.id,
            customerName: customer.name,
        }
    };

    docClient.put(params, function (err, data) {
        if (err) {
            // Faild do add customer
            var serverError = new makeError(500, `Unable to add customer: ${customer.name}. Error` + JSON.stringify(err, null, 2));
            callback(serverError);
            //console.error("Unable to add customer", customer.name, ". Error JSON:", JSON.stringify(err, null, 2));
        } else {
            console.log("PutItem succeeded:", customer.name);
            callback(null);
        }
        //console.log("-------------------------------------------\n");
    });
}

function alreadyExists(requestName, callback) {
    var docClient = new AWS.DynamoDB.DocumentClient();

    var params = {
        TableName: tableName,
        IndexName: indexCustomerName,
        KeyConditionExpression: "#name = :reqName",
        ExpressionAttributeNames: {
            "#name": customerName
        },
        ExpressionAttributeValues: {
            ":reqName": requestName
        }
    };

    docClient.query(params, (err, data) => {
        if (err) {
            // Query faild
            var serverError = new makeError(500, 'Unable to query. Error' + JSON.stringify(err, null, 2));
            callback(serverError);
        } else {
            if (data.Items.length != 0) {
                // A customer with the requested name already exists
                var notUniqueNameError = new makeError(400, `customer name: ${requestName} has already been used.`);
                callback(notUniqueNameError);
            }
            // The requested customer name is unique
            else {
                console.log('ok4');
                callback(null);
            }
        }
    });
}

app.post('/api/customers', (req, res) => {
    // create a joi schema for input validation
    const schema = {
        customerName: Joi.string().required(),
    };
    //validation result
    const result = Joi.validate(req.body, schema);
    //bad requet - 400
    if (result.error) {
        sendFailure(res, new makeError(400, result.error.details[0].message));
        return;
    }

    // Check customer name doesn't already exist
    alreadyExists(req.body.customerName, (err) => {
        if (err) {
            sendFailure(res, err);
        }
        else {
            const customer = {
                id: String(uuidv1()), // The id type is set to 'string' in the db 
                name: req.body.customerName
            };
            //add the new customer to db
            addCustomer(customer, (err) => {  // CHECK!!!! Maybe problem with seeing req, res objects from callback within callback
                if (err) {
                    sendFailure(res, err);
                }
                else {
                    //send the newly stored object back to client
                    sendSuccess(res, customer.name);
                }
            });

        }
    });
});


// ------------------------------ PUT ------------------------------

function exist(targetName, callback) {
    var docClient = new AWS.DynamoDB.DocumentClient();

    var params = {
        TableName: tableName,
        IndexName: indexCustomerName,
        KeyConditionExpression: "#name = :trgName",
        ExpressionAttributeNames: {
            "#name": customerName
        },
        ExpressionAttributeValues: {
            ":trgName": targetName
        }
    };

    docClient.query(params, (err, data) => {
        if (err) {
            // Query faild
            var serverError = new makeError(500, 'Unable to query. Error' + JSON.stringify(err, null, 2));
            callback(serverError, null);
        } else {
            if (data.Items.length == 0) {
                // No such customer
                var noCustomerError = new makeError(404, `customer name: ${targetName} does NOT exist.`);
                callback(noCustomerError, null);
            }
            // Return customer id
            else {
                callback(null, data.Items[0].customerId);
            }
        }
    });
}

function alterCustomer(targetId, newName, callback) {
    var docClient = new AWS.DynamoDB.DocumentClient();

    var params = {
        TableName: tableName,
        Key: {
            customerId: targetId
        },
        UpdateExpression: "set #oldName = :newName",
        ExpressionAttributeNames: {
            "#oldName": customerName
        },
        ExpressionAttributeValues: {
            ":newName": newName
        },
        ReturnValues: "UPDATED_NEW"
    };
    docClient.update(params, function (err, data) {
        if (err) {
            var serverError = new makeError(500, 'Unable to update customer. Error' + JSON.stringify(err, null, 2));
            callback(serverError);
        } else {
            callback(null);
        }
    });
}

app.put('/api/customers/:id', (req, res) => {
    // Look up the customer
    // If doesn't exist, return 404
    const targetName = req.params.id.toString();
    exist(targetName, (err, targetId) => {
        if (err) {
            sendFailure(res, err);
        }
        else {
            // Validate
            const schema = {
                customerName: Joi.string().required(),
            };
            const result = Joi.validate(req.body, schema);
            //bad requet - 400
            if (result.error) {
                sendFailure(res, new makeError(400, result.error.details[0].message));
                return;
            }

            // Update customer
            // Update requiered fields here
            const newName = req.body.customerName;
            alterCustomer(targetId, newName, (err) => {
                if (err) {
                    sendFailure(res, err);
                }
                else {
                    // Return the uptdated customer
                    sendSuccess(res, newName);
                }
            })
        }
    });
});


// ------------------------------ DELETE ------------------------------

function deleteCustomer(targetId, targetName, callback) {
    var docClient = new AWS.DynamoDB.DocumentClient();

    var params = {
        TableName: tableName,
        Key: {
            customerId: targetId
        }
    };

    docClient.delete(params, function (err, data) {
        if (err) {
            var serverError = new makeError(500, `Unable to delete customer: ${targetName}. Error` + JSON.stringify(err, null, 2));
            callback(serverError);
        } else {
            callback(null);
        }
    });
}

app.delete('/api/customers/:id', (req, res) => {
    const targetName = req.params.id.toString();
    exist(targetName, (err, targetId) => {
        if (err) {
            sendFailure(res, err);
        }
        else {
            // delete
            deleteCustomer(targetId, targetName, (err) => {
                if (err) {
                    sendFailure(res, err);
                }
                else {
                    // Return deleted customer
                    sendSuccess(res, targetName);
                }
            });
            // Return the deleted customer
        }
    });
});


// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

console.log("web-api running ...");

//use the port number of the PORT env variable otherwise use 3000
const port = process.env.port || 3000;

//listen on port
console.log(`connecting to port: ${port} ...`);
app.listen(port, () => console.log(`listening on port ${port} ...`));