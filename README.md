# DotNetCore1

Ground up walkthru of Creating .NET Core/Entity Framework CRUD End Point with Swagger, Autofac, Sql Server and both In Memory and Live DBase for Testing.


### 1. Create Base .Net Core Web App in Visual Studio
### 2. Create Unit Test Project
### 3. Add Nuget Packages

1.	Microsoft.EntityFrameworkCore (Add Microsoft.AspNetCoreAll)
2.	Swashbuckle.AspNetCore
3.	Autofac.Extensions.DependencyInjection
4.	Moq (Add to Unit Test Project)
5.	Run Project and Test

### 4. Wire up Swagger

1.	Add Code Lines in Startup.cs
2.	Change Project Properties setting (Launch Browser "swagger")
3.	Run Project and Test

### 5. Add Entity Framework Models and Database Create

1.	Add ConnectionString to appsettings.json
2.	Add Models and Context
3.	Edit Startup.cs to wire up context/migrations
4.	Run Project
5.	Add-Migration "initial"
6.	Update-Database -verbose
7.	Run Project and Test

### 6. Add Controller/Service/Interface/Autofac

1.	Add Controller/Service/Interface and Build Project
2.	Wire up Autofac and run project (Startup.cs ConfigureServices edit to return IServiceProvider)
3.	Run Project and Test


## Authors

* **Michael Bruno** 

See also the list of [contributors](https://github.com/your/project/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Hat tip to anyone whose code was used
* Inspiration
* etc
