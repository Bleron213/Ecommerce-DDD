<h1 align="center">
  <br>
  <br>
    ECommerce DDD Application
  <br>
</h1>

<h4 align="center"> A DDD-Based Web API for handling Orders</h4>

<p align="center">

## Key Features

* ASP.NET 9.0
* Mediator and CQRS pattern
* Clean Architecture
* Domain-Driven Design
* Cross Platform
* Global Exception Handling
* Structured Logging
* Fully compliant with Dependency Injection
* Dockerized

## Technology Stack

* ASP.NET Core 9.0 Web API
* Postgresql 

## How To Use

* Clone the git repo locally

```bash
# Clone this repository
$ git clone https://github.com/Bleron213/Ecomerce-DDD
```

# How to run in your local machine
* Open the solution in Visual Studio 2022
* Start Ecommerce.API API. Make sure Postgres is running and accepting connections. Adjust the credentails in appsettings.Development.json as needed.

# How to run in Docker
* Navigate to the directory where the solution is
* Make sure you have Docker installed and running
* Open a terminal and type **docker-compose up**

## How To Test

1. In the solution root, you can find a postman collection. Open it in Postman and you can send test requests. Make sure to adjust ports as needed.
2. Open View -> Test Explorer -> Right click on Ecommerce.Tests and Run. Everything should pass.

## Design Decisions & Assumptions

### Repository Pattern

The Repository pattern has been implemented in this solution for PoC reasons, but I don't think it is necessary or recommended. Repository pattern and Specification pattern are abstractions that provide no real benefit other than bringing in another abstraction. EF Core is already a Repository pattern with Unit Of Work, so introducing another Repository pattern is unnecessary and redundant. It would make sense if the ORM was Dapper or ADO.NET, but EF Core is already a very stable software and abstracting it away makes it less powerful.

In Production, I would argue that just using DbContext directly provides more value rather than creating another abstraction.

### Unit Testing Approach

**Unit Tests** are strictly implemented in the Entity Level layer. No mocking has been implemented and I don't recommend any mocking, ever. Mocking provides zero tangible value. Unit tests here test the business logic strictly, and if a refactor happens in the future or the current business logic is extended, we can be sure that our previous logic remains as is. There is real value from these tests because 

1. They are blazing fast.
2. They can run in the CI/CD pipeline and provide fast feedback if something breaks.

### Code-First vs Database-First

Initially, I wanted to add a Database-First approach, but for PoC reasons I decided to go with a Code-First application. In Production-grade applications, however, I recommend going Database-First.

  
## Credits

This software uses the following open source software in its source code:

- [.ASP.NET Core 9](https://github.com/dotnet)
- [MediatR](https://github.com/jbogard/MediatR)
- [EF Core](https://github.com/efcore)
- [NUnit](https://github.com/nunit/nunit)
- [Serilog](https://github.com/serilog/serilog)
- [Docker](https://github.com/docker)
