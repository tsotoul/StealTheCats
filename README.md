# Steal The Cats Api

An ASP.NET Web APO for fetching cats from the free https://api.thecatapi.com/, built in Visual Studio with Microsoft SQL Server as Database Storage and Entity Framework Core as ORM

## Table of Contents

- [Description](#description)
- [Setup API](#setupApi)
- [Setup database](#setupDatabase)
- [Usage](#usage)
- [Potential Improvements](#improvements)

## Description
### Endpoints:
The Web Api consists of the following 4 endpoints:
- **POST /api/cats/fetch**: Endpoint to retrieve 25 (by default) cats from the https://api.thecatapi.com/ and save them to the database. The endpoint is designed to fetch 25 cats **every** time and makes sure that no duplicates are inserted in the database. It will also save the Tags of the cats, also making sure that no duplicate Tags are inserted. Also, it handles the connection between the cat and its Tags.
- **GET /api/cats/{id}**: Endpoint to retrieve the Cat with the specified Id from the database.
- **GET /api/cats**: Endpoint to retrieve cats with pagination support from the database*.
- **GET /api/cats**: Endpoint to retrieve cats by their Tag with pagination support from the database*.

  \* *The 3rd and 4th endpoints are merged into one endpoint, as the endpoint url required to be the same. They can work autonomously (ex. if you want cats not filtered by Tag, you need to leave the Tag field empty).*

### Design
The Api is designed following the multilayer repository pattern meaning:
- No businees logic in the Controllers<br />
- One business logic layer (CatsService)
- One repository responsible to communicate with the Cats Api (CatApiRepository)
- One repository responsible to communicate with the database (DatabaseRepository)

### Unit tests
Unit tests are included for the Controller, Service and Repositories

### Dependencies/Packages used
- [AutoMapper](https://automapper.org/)
- [EntityFrameworkCore](https://www.nuget.org/packages/microsoft.entityframeworkcore)
- [EntityFrameworkCore InMemory](https://www.nuget.org/packages/microsoft.entityframeworkcore.inmemory)
- [EntityFrameworkCore SQLServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.sqlserver/)
- [EntityFrameworkCore Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools)
- [Swagger](https://www.nuget.org/packages/swashbuckle.aspnetcore.swagger/)
- [NUnit](https://www.nuget.org/packages/nunit/)
- [NSubstitute](https://www.nuget.org/packages/nsubstitute/)
- [Shouldly](https://www.nuget.org/packages/shouldly/)

## Setup API
To use the API, you need to have the API secret. <br>
Go to https://api.thecatapi.com/ and follow the instructions to get an api secret in your personal email.
Once you have it, locate the **appSettings.json** in the rool folder of the project, and replace *"YOUR_API_SECRET_GOES_HERE"* with your api secret.
