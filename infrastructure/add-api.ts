import * as gcp from "@pulumi/gcp";


export default class AddService extends gcp.cloudrun.Service {
    constructor(location: string) {
        super('add-api', {
            name: 'add-api',
            location: location,
            template: {
                spec: {
                    serviceAccountName: "add-api@gcp-pocket.iam.gserviceaccount.com",
                    containers: [
                        {
                            image: "gcr.io/gcp-pocket/add-api",
                            ports: [
                                {
                                    containerPort: 80
                                }
                            ]
                        },

                        
                    ],
                },
                metadata: {
                    annotations: {
                        "run.googleapis.com/ingress": "internal"
                    }
                }
            }
        })

        new gcp.cloudrun.IamMember("add-api-allow-calculator-api", {
            service: this.name,
            location,
            role: "roles/run.invoker",
            member: "serviceAccount:calculator-api@gcp-pocket.iam.gserviceaccount.com",
        });
    }
}
