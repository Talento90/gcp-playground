import * as pulumi from "@pulumi/pulumi";
import * as gcp from "@pulumi/gcp";

import CalculatorApi from "./calculator-api";
import AdditionApi from "./add-api";
import SubtractionApi from "./sub-api";

const location = gcp.config.region || "europe-west3";


var calculatorSA = new gcp.serviceaccount.Account("calculator-api", {
    accountId: "calculator-api"
})

var addSA = new gcp.serviceaccount.Account("add-api", {
    accountId: "add-api"
})

var subSA = new gcp.serviceaccount.Account("sub-api", {
    accountId: "sub-api"
})

new gcp.compute.Network('vpc-0', {
   name: 'vpc-0',
   autoCreateSubnetworks: true, 
})

new gcp.vpcaccess.Connector('vpc-0-connector', {
    name: 'vpc-0-connector',
    network: 'vpc-0',
    ipCidrRange: '10.8.0.0',
})

var add = new AdditionApi(location);
var sub = new SubtractionApi(location);

var addApiUrl: string = add.statuses[0].url as unknown as string;
var subApiUrl: string = sub.statuses[0].url as unknown as string;

var calc = new CalculatorApi(location, addApiUrl, subApiUrl);


// var apiGateway = new gcp.apigateway.Api('calculator-api-gateway', {
//     apiId: 'calculator-api-gateway',
//     displayName: 'Api Gateway for calculator-api',  
// })


// new gcp.apigateway.Gateway('calculator-api-gateway', {
//     gatewayId: 'calculator-api-gateway',
//     apiConfig: ""
// })