# Payment Gateway

This repository includes sample payment which can be used by merchants to make payment request. The api projects are written in .Net Core 3.1 where as non API project are targeting NetStandard 2.1

## Features/Technologies

This application uses following technology:

### Docker for Containerization

For cross-platform compatibility docker container are used. 

**Following containers spin up when "docker-compose up --build" is run:**

##### _Seq_
Seq is used as log server where all log message are visible and can be found at http://localhost:5340

##### _RabbitMQ_
Currently RabbitMQ is used as message broker for communication between microservices (in our case dispatching integration events between MerchantIdentity and PaymentGateway) and can be found at http://localhost:15672/

##### _MsSqlServer_
MsSqlServer is used for data persistence. Currently two database are generated when the application is run, one for each microservice.

##### _Prometheus_
Prometheus is a time series database which is used for collection application metrics such as http requests, application healthcheck, database health check. UI for prometheus can be found at http://localhost:9090

##### _Grafana_
Metrics that are collected through prometheus can be visualised in Grafana. Alerts can be set as per requirement. Grafana UI can be found at http://localhost:3001

##### _MerchantIdentity_

MerchantIdentity is a microservice responsible to creating new service and generating Public Key / Private key for each merchant. Merchant API runs at http://localhost:5110

##### _PaymentGateway_
PaymentGateway is a microservice responsible for handling payments and validating card via MockBank. PaymentGateway API runs at http://localhost:5111


### Application Logging
Log messages are logged to seq server using [Serilog](https://serilog.net/).
Following behaviours are always logged:

- Exceptions via `HttpGlobalExceptionFilter`
- Invalid client requests via `FluentValidation`
- Each command's request and result
- Database read or write duration for each request

### Authentication
Private key that are generated during merchant registration in MerchantIdentity should be used while making requests to PaymentGateway API. These public key and private key are generated using `RSACryptoProvider` of 512 _keylength_. The secret key should be added in header with `Authorization` as key. Missing or invalid private key will result in 401(Unauthorized) response. For easier viewing and validation purposes public keys 


### Performance testing (Could be adapted as per requirement)
Simple Performance testing can be carried out by using third party tools such as [Bombardier](https://github.com/codesenberg/bombardier).
To make sure application is performant production environment data such as number of request at peak hours and container memory size can be taken into consideration. This data can be used during local performance test to make sure things run smoothly.

Following sample makes 200 connection to the endpoint and makes 100 requests per second for 10 seconds.

`bombardier -c 200 -d 10s -l --rate 200 https://localhost:5111/api/payments/getAllPayments`

### Idempotency Handling
Idempotency handling is handled by assigning `CommandId` for each request (Command). CommandId comprises of command prefix, unique field representating request and timestamp. `ClientRequest` are stored in Requests table in database. Before continuing any commands from client idempotency check is carried out. ClientRequest idempotency check is handled in infrastructure layer.

### Mock Risk Analysis
Currently as per requirement MockBankService is implemented which could be replaced with real bank service. However, a mock risk analysis service is also implemented. Currently there is a possibility to provide `OriginIpAddress` while making payment request. This field is optional, but, if the field is available a RiskAnalysis is made. Current implementation of `MockRiskAnalysis` service returns the request is 'not fraudulent' with probablity of 0.95.

## Application architecture, patterns and design choices
Both the services have four distinc layers. This sort of layered architecture might not always be necessary but it does provide Seperation of concern and layers can be swapped based upon the requirements in future.

#### -Domain Layer (Domain driven design)
Domain layer consists of domain models, domain events as well as Aggregate root's interfaces. For this implementation 3 aggreate roots are created, namely : `Card`, `Merchant` and `Payment`. Only aggregate root's are allowed to raise DomainEvents. Each aggregate root has its own Repository interface. Non-aggreate roots and defined as value objects and are owned by aggregate roots. Payment aggregate root in current implementation owns `IdPaymentSource`, `BillingAddress`, `PaymentRecipient`, `Notification` and collection of `MetaData`. 

#### -Infrastructure Layer
Infrastructure layer contains implementation of Aggregrate Root's repository and data persistence. Both the services currently use MsSql database for data persistence. Infrastructure layer's repository implementation uses EntityFramework as ORM tool for data persistence. This layer is also responsible for dispatching `DomainEvent` that were raised by domain models.

Domain events in this particular implementation is dispatched before changes to domain models are commited. 

Other option could have been to dispatch the events after the changes were commited which means eventual consistency should be handled accordingly in case any handlers fail to handle the domain events.

#### -Application layer
Application layer is responsible to receiving Api layer's request and handling the request accordingly and providing relevant response.

#### -API layer 
Api layer is responsible to receiving the requests by client/frontend and forwarding the request to the right handler.

## Optimization and further improvements
Here are optimization that could be made in future for better performance, scalability and security.

#### API Gateway/BFF(Backend for Frontend)
Api endpoints in docker container are not HTTPS and should not be directly accessible to clients. Therefore, an API gateway which act as a proxy between user request and api should be provided. Some existing API gateway are : AWS API GateWay, Kong API, Nginx.

#### Extract card service to its own microservice
Card endpoint lets user store bank card information and retreive card identifier. CardService also calls the bank service to validate the card is valid. This service could be extracted to its own service and send a message with tokenized card info and card identifier to PaymentGateway when new card is successfully added. 

#### NoSql database for read purpose.
Any user request that needs data persistence could be save in SQL and NoSQL database.NoSQL database data could involve data based on future user data read request. This could provide complication in managing data consistency but will improve read performance.

#### End to end encryption
Each merchant is assigned with a private and public key. Request and response could be encrypted by merchant's public key which can only be decrypted by merchant's private key.

#### Domain validations
Data validation is heavily reliant on user's input validation, which is done via `FluentValidation` library. Invalid request are returned with `BadRequest` result before entering the controller's method. 

However, this validation should be extended to domain level too. Since the core business logic and how a application should work is contained in Domain layers validating the inputs in domain layer is essential.

### Testing
Currently only few test cases are written. These tests are meant to be a blueprint for other further tests.