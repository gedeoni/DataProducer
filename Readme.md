## Data Producer
This is a simple dotnet project that consists of two microservices: 
- A dotnet background service
- An api to allow accessing produced data


## Technology integration
A worker is used to dynamically generate data
Sqlite is used to store the generated data
A payload containing the url to the generated content is then published to RabbittMQ
The Api microservice is then used to access the generated data

## Architecture
The Project is largely inspired by the Clean Architcture also known as Onion Architectue and uses the repository Pattern to better encapsulate the database access logic.

## Installation
Note that you will need to have RabbitMq installed
git clone https://github.com/gedeoni/DataProducer.git
cd DataProducer
dotnet restore
dotnet run -p ClientProducer #start the Worker
dotnet run -p ClientApi #start the Api
