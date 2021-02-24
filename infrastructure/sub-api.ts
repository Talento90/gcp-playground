import * as gcp from "@pulumi/gcp";

export default class SubService extends gcp.cloudrun.Service {
    constructor(location: string) {
        super('sub-api', {
            name: 'sub-api',
            location: location,
            template: {
                spec: {
                    serviceAccountName: "sub-api@gcp-pocket.iam.gserviceaccount.com",
                    containers: [
                        {
                            image: "gcr.io/gcp-pocket/sub-api",
                            ports: [
                                {
                                    containerPort: 80
                                }
                            ]
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

        new gcp.cloudrun.IamMember("sub-api-allow-calculator-api", {
            service: this.name,
            location,
            role: "roles/run.invoker",
            member: "serviceAccount:calculator-api@gcp-pocket.iam.gserviceaccount.com",
        });

    }
}
