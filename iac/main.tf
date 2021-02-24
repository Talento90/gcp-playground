terraform {
  required_providers {
    google = {
      source = "hashicorp/google"
      version = "3.5.0"
    }
  }
}

provider "google" {
  credentials = file(var.credentials)
  project = var.project
  region  = var.region
}

resource "google_compute_network" "vpc_network" {
  name = "vpc-0"
}

resource "google_cloud_run_service" "calculator-api" {
  name = "calculator-api"
  location = var.region
  project = var.project

  template {
    spec {
      containers {
        image = "gcr.io/gcp-pocket/calculator-api"
        env {
          name = "PORT"
          value = "80"
        }
      }
      service_account_name = "calculator-api@gcp-pocket.iam.gserviceaccount.com"
    }
  }

  traffic {
    percent         = 100
    latest_revision = true
  }
}


resource "google_cloud_run_service" "sub-api" {
  name = "sub-api"
  location = var.region
  project = var.project

  template {
    spec {
      containers {
        image = "gcr.io/gcp-pocket/sub-api"
        env {
          name = "PORT"
          value = "80"
        }
      }
      service_account_name = "sub-api@gcp-pocket.iam.gserviceaccount.com"
    }
  }

  traffic {
    percent         = 100
    latest_revision = true
  }
}

resource "google_cloud_run_service" "add-api" {
  name = "add-api"
  location = var.region
  project = var.project

  template {
    spec {
      containers {
        image = "gcr.io/gcp-pocket/add-api"
        env {
          name = "PORT"
          value = "80"
        }
      }
      service_account_name = "add-api@gcp-pocket.iam.gserviceaccount.com"
    }
  }

  traffic {
    percent         = 100
    latest_revision = true
  }
}