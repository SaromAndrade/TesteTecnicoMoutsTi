# Developer Evaluation Project
This repository contains the solution for the technical test proposed by Mouts TI. It's a backend application with PostgreSQL database integration.

## Prerequisites

- Docker
- Docker Compose

## Getting Started

### Clone the Repository

First, clone this repository to your local machine:

```bash
git clone https://github.com/SaromAndrade/TesteTecnicoMoutsTi.git
cd TesteTecnicoMoutsTi

### Navigate to the Docker Compose Directory
```sh
cd template/backend
```
### Start the Containers
Run the following command in the `backend` directory:
```sh
docker-compose up -d --build
```
This will start:
. The main web API service (running on port 8080)
. PostgreSQL database (running on port 5423)

### Access the Application
Once the containers are running, you can access the application at:
- **API Service:** http://localhost:8080
The API should now be available for requests.

### Stop the Containers
To stop the running services:
```sh
docker-compose down
```
### Architecture Overview
The solution uses:
- ASP.NET Core Web API
- PostgreSQL relational database
- Docker containers for easy deployment

### Configuration
The Docker Compose file configures:
- Web API on ports 8080 (HTTP) and 8081 (HTTPS)
- PostgreSQL database with:
- Database: developer_evaluation
- Username: developer
- Password: senha123
- Exposed on port 5423

### Notes
The database will persist data between container restarts
Make sure ports 8080 and 5423 are available on your machine

