#!/bin/bash

$PROJECT_ID = "gcp-pocket"
$SERVICE_NAME = "calculator-api"
$REGION = "europe-west3"
$IMAGE = gcr.io/${PROJECT_ID}/${SERVICE_NAME}

gcloud builds submit --tag $IMAGE
gcloud run deploy --image $IMAGE --platform managed --region ${REGION}


gcloud builds submit --tag gcr.io/gcp-pocket/calculator-api

gcloud run deploy --image gcr.io/gcp-pocket/calculator-api --platform managed --region europe-west3

