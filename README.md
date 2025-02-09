# Steal The Cats Api

An ASP.NET Web API for fetching cats from the free https://api.thecatapi.com/, built in Visual Studio with Microsoft SQL Server as Database Storage and Entity Framework Core as ORM

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
To use the API, you need to have the Cats API secret. <br>
Go to https://api.thecatapi.com/ and follow the instructions to get an api secret in your personal email.
Once you have it, locate the **appSettings.json** in the rool folder of the project, and replace *"YOUR_API_SECRET_GOES_HERE"* with your api secret.

## Setup database
To setup the database, you will need to have SQL Server Management Studio or equivalent, or use any interface of your Microsoft SQL Server<br>
Once you open it, create an empty database called Cats and copy the Server name in the Data Source of your **appSettings.json** replacing *"YOUR_SERVER_NAME_GOES_HERE"*.<br>
Next, follow 1 of the options below:<br>

### Option 1
1: Open Visual Studio and load the project.<br>
2: Go to Tools -> Nuget Package Manager -> Package Manager Console. This will open the Package Manager console with a prompt to write your command.<br>
3: Run the following command: **Update-database** (you need Microsoft.EntityFrameworkCore.Tools for this).

### Option 2
1: Locate the **SchemaCreate.sql** file in the root folder of the project and copy everything (Ctrl+A on windows)<br>
2: Open the Query to run an SQL script targetting Cats database<br>
3: Paste the SQL commands you have copied and Run the query<br>


In both options, you should be able to see the tables created in your local database like this:<br>
![image](https://github.com/user-attachments/assets/6fcf8135-576f-4aa3-8277-ebbb83afaab2)

<p>The database consist of the following tables and columns:<p>
  
**Cats**: Table to store the cats with the following columns:<br>
• *Id*: An auto incremental unique integer that identifies a cat within your
database<br>
• *CatId*: Represents the id of the image returned from CaaS API<br>
• *Width*: Integer of the width of the image returned from CaaS API<br>
• *Height*: Integer of the height of the image returned from CaaS API<br>
• *Image*: Image data in VarBinary formate (byte[])<br>
• *Created*: Timestamp of creation of database record <br>

**Tags**: Table to store unique tags (in capitals) with the following columns:<br>
• *Id*: An auto incremental unique integer that identifies a tag<br>
• *Name*: The name of the tag<br>
• *Created*: Timestamp of creation of database record <br>

**CatTags**: Table to store the relationship between the cats and the tags with the following columns:<br>
• *Id*: An auto incremental unique integer that identifies a CatTag<br>
• *CatId*: The Cat.Id<br>
• *TagId*: The Tag.Id<br>

**MigrationHistory**: Table to store the migrations ran in the database:<br>

![image](https://github.com/user-attachments/assets/54c45991-abfa-4f7d-aa16-4340f5fe9661)

## Usage
Once everything is set up, Run the application locally from Visual Studio (or other IDE) and you will automatically be prompted with the Swagger endpoint where you can test and run your application.<br>
![image](https://github.com/user-attachments/assets/ef0587e2-4442-44a2-a352-7e7c17f03c54)

## Potential Improvements
The application can be improved in the following areas:
- Create integration tests to test the connections with the Cats Api
- Set Identities in the database to start from the last existing Id as now, if you have Ids = [1, 2, 3, 4, 5, 6] and you remove records with Ids 5 and 6, the next fetch will start at Id = 7
- More unit test cases
- Handle failures better for Interal Server Error of the Cats Api
- Perform more validations on the database properties with NAVCHAR(255) for text
- Add database Indexes
