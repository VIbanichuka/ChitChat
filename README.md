## A Messaging application with ASP.NET Core, Angular and SignalR (Ongoing project)

PrimaMensa Chitchat features:
-[**ChitChat.App**](src/ChitChat.App/) - An ASP.NET Core Web API with Angular project

## Architecture
- Clean Onion architecture

This application is separated into 4 layers of architecture:
- The Domain layer [**ChitChat.Core**](src/ChitChat.Core/)
- The Application layer [**ChitChat.Application**](src/ChitChat.Application/)
- The Infrastructure layer [**ChitChat.Infrastructure**](src/ChitChat.Infrastructure/)
- The Api layer [**ChitChat.App.Server**](src/ChitChat.App/ChitChat.App.Server)
- The Presentation layer[**ChitChat.App.Client**](src/ChitChat.App/chitchat.app.client)
- Tests[**ChitChat.Test**](tests/UnitTests)

For information about [Clean Onion architecture](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)

## Design Pattern
- Generic Repository Pattern
- Dependency Injection Design Pattern
- The MVC (Model-View-Controller) architectural Design Pattern

For information about [MVC pattern](https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-7.0) and [DI pattern](https://dotnettutorials.net/lesson/dependency-injection-design-pattern-csharp/)

This application showcases:
- [SignalR](https://learn.microsoft.com/en-us/aspnet/signalr/overview/getting-started/introduction-to-signalr) for two way communication
- ASP.NET Core Web Api 
- EntityFramework Core
- Postgresql
- [AutoMapper](https://docs.automapper.org/en/latest/Getting-started.html) for mapping one object to another.
- LINQ
- JWT for authorisation and authentication
- Redis as a datastore
- Docker for hosting redis image

## Prerequisites

### .NET
1. [Install .NET 7](https://dotnet.microsoft.com/en-us/download)

### Database
1. Install the **dotnet-ef** tool
2. Install the Postgresql
3. Install PgAdmin

## Demo Video
https://github.com/VIbanichuka/ChitChat/assets/94909597/f1e63228-880c-470d-904d-eece1aebfa83