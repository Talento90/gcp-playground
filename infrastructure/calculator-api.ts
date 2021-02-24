import * as gcp from "@pulumi/gcp";

export default class CalculatorService extends gcp.cloudrun.Service {
    constructor(location: string, addApi: string, subApi: string) {
        super('calculator-api', {
            name: 'calculator-api',
            location: location,
            template: {
                spec: {
                    serviceAccountName: "calculator-api@gcp-pocket.iam.gserviceaccount.com",
                    containers: [
                        {
                            image: "gcr.io/gcp-pocket/calculator-api",
                            ports: [
                                {
                                    containerPort: 80
                                }
                            ],
                            envs: [
                                {
                                    name: "ADD_API",
                                    value: addApi
                                },
                                {
                                    name: "SUB_API",
                                    value: subApi
                                }
                            ],
                        }
                    ],
                },
                metadata: {
                    annotations: {
                        "run.googleapis.com/ingress": "internal"
                    }
                }
            }
        })

        new gcp.cloudrun.IamMember("calculator-api-allow-all-users", {
            service: this.name,
            location,
            role: "roles/run.invoker",
            member: "allUsers",
        });
    }
}
