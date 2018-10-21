
using DotNetCoreMM.Controllers;
using DotNetCoreMM.Models;
using DotNetCoreMM.Services;
//using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;



public class EmployeeControllerTests
{

    private string curDatabaseType;

    private EmployeeController _controller;
   // private EmployeeController _controllerLive;
    private readonly IEmployeeService _service;
    private MMDatabaseContext context;
   // private MMDatabaseContext contextLive;

    protected EmployeeService PersonService;
    protected Mock<MMDatabaseContext> MockContext;
    protected Mock<DbSet<Employee>> MockSet;

    public EmployeeControllerTests()
    {
    }

    [Fact]
    public void A_Controller_Get_WhenCalled_ReturnsOkResult()
    {
        if (curDatabaseType != "InMemory") { Setup(); };
        // Act
        var okResult = _controller.Get();

        // Assert
        Assert.IsType<OkObjectResult>(okResult);
    }

    [Fact]
    public void A_Controller_Get_WhenCalled_ReturnsAllItems()
    {
        if (curDatabaseType != "InMemory") { Setup(); };
        // Act
        var okResult = _controller.Get() as OkObjectResult;

        // Assert
        var items = Assert.IsType<List<Employee>>(okResult.Value);
        Assert.Equal(3, items.Count);
    }

    [Fact]
    public void A_Controller_GetById_WhenCalled_ReturnsOkResult()
    {
        if (curDatabaseType != "InMemory") { Setup(); };
        // Act
        var okResult = _controller.Get(1).Result;

        // Assert
        Assert.IsType<OkObjectResult>(okResult);
    }

    [Fact]
    public void A_Controller_GetById_UnknownId_ReturnsNotFoundResult()
    {
        if (curDatabaseType != "InMemory") { Setup(); };
        // Act
        var notFoundResult = _controller.Get(40).Result;

        // Assert
        Assert.IsType<NotFoundResult>(notFoundResult);
    }

    [Fact]
    public async void A_Controller_GetById_WhenCalled_ReturnsOneItem()
    {
        if (curDatabaseType != "InMemory") { Setup(); };
        // Act
        var okResult = await _controller.Get(1) as OkObjectResult;

        // Assert
        var item = Assert.IsType<Employee>(okResult.Value);
        Assert.Equal("a", item.FirstName);
    }

    [Fact]
    public void B_Context_Add_Success()
    {
        if (curDatabaseType != "InMemory") { Setup(); };
        
        // Act
        context.Add(new Employee { id = 4, FirstName = "ControllerTest", LastName = "b", Email = "dd", PhoneNumber = "1234567898" });
        context.SaveChanges();
        var result = context.Employees.Find(System.Convert.ToInt64(4));

        // Assert
        var item = Assert.IsType<Employee>(result);
        Assert.Equal("ControllerTest", item.FirstName);

    }
    

    [Fact]
    public void B_Context_Put_Success()
    {
        //Arrange
        if (curDatabaseType != "InMemory") { Setup(); };
        //retrieve test record
        var editRec = context.Employees.Find(System.Convert.ToInt64(3));
        var editRecItem = (Employee)editRec;

        // Act
        editRec.FirstName = "PutSuccess";
        context.Update(editRec);
        context.SaveChangesAsync();
        var result = context.Employees.Find(System.Convert.ToInt64(3));

        // Assert
        Assert.Equal("PutSuccess", result.FirstName);

    }
    [Fact]
    public void B_Context_Delete_Success()
    {
        //Arrange
        Setup();  //add 3 records
        //retrieve test record
        var deleteRec = context.Employees.Find(System.Convert.ToInt64(3));
        var deleteRecItem = (Employee)deleteRec;

        // Act
        context.Remove(deleteRecItem);
        context.SaveChanges();
        var result = context.Employees.ToList();

        // Assert
        Assert.Equal(2, result.Count);

    }

    [Fact]
    public void C_Service_ShouldAddNewEmployee()
    {

        using (var cxt = new MMDatabaseContext(new DbContextOptions<MMDatabaseContext>(),false))
        {
            // Arrange
            var service = new EmployeeService(cxt);
          
            //    // Arrange
            Employee testItem = new Employee()
            {
       
                FirstName = "service",
                LastName = "b",
                Email = "dd",
                PhoneNumber = "1234567898"

            };

            // Act
            var result = service.AddAsync(testItem);

            // Assert
            var item = Assert.IsType<Employee>(result.Result);
            Assert.Equal("service", item.FirstName);

        }

    }


    [Fact]
    public async void C_Service_ShouldUpdateEmployee()
    {
        using (var cxt = new MMDatabaseContext(new DbContextOptions<MMDatabaseContext>(), false))
        {
            // Arrange
            var addRec = new Employee() { FirstName = "UpdateMe", LastName = "b", Email = "dd", PhoneNumber = "1234567898" };
            cxt.Add(addRec);
            cxt.SaveChanges();
            var addId = addRec.id;
            var service = new EmployeeService(cxt);
            var updateRec = (Employee)addRec;
            updateRec.FirstName = "ImUpdated";

            // Act
            var update = await service.UpdateAsync(updateRec);

            // Assert
            var ret = service.GetByIdAsync(addId).Result;
            Assert.Equal("ImUpdated", ret.FirstName);


        }

    }

    [Fact]
    public void C_Service_ShouldDeleteEmployee()
    {

        using (var cxt = new MMDatabaseContext(new DbContextOptions<MMDatabaseContext>(), false))
        {
            // Arrange
            var addRec = new Employee() { FirstName = "DeleteMe", LastName = "b", Email = "dd", PhoneNumber = "1234567898" };
            cxt.Add(addRec);
            cxt.SaveChanges();
            var addId = addRec.id;
            var service = new EmployeeService(cxt);
            var deleteRec = (Employee)addRec;

            // Act
            service.Delete(deleteRec);

            // Assert
            var ret = service.GetByIdAsync(addId).Result;
            Assert.Null(ret);


        }

    }


    [Fact]
    public void D_Integration_ShouldAddNewEmployee()
    {

        using (var cxt = new MMDatabaseContext(new DbContextOptions<MMDatabaseContext>(), false))
        {
            // Arrange
            var service = new EmployeeService(cxt);
            var controller = new EmployeeController(service);
            Employee testItem = new Employee()
            {

                FirstName = "integration",
                LastName = "b",
                Email = "dd",
                PhoneNumber = "1234567898"

            };

            // Act
           var result = (OkObjectResult)controller.Post(testItem).Result;
           var item = (Employee)result.Value;

            // Assert
           Assert.Equal("integration", item.FirstName);


        }

    }

    [Fact]
    public void D_Integration_ShouldUpdateEmployee()
    {
        using (var cxt = new MMDatabaseContext(new DbContextOptions<MMDatabaseContext>(), false))
        {
            // Arrange
            var service = new EmployeeService(cxt);
            var controller = new EmployeeController(service);
            Employee testItem = new Employee()
            {

                FirstName = "UpdateMe",
                LastName = "b",
                Email = "dd",
                PhoneNumber = "1234567898"

            };

            // Act
            var postTestRec = (OkObjectResult)controller.Post(testItem).Result;
            var item = (Employee)postTestRec.Value;
            item.FirstName = "ImNowUpdated";
            var result =  (OkObjectResult)controller.Put(testItem.id, testItem).Result;

            // Assert
            Assert.Equal("ImNowUpdated", item.FirstName);


        }

    }

    [Fact]
    public void D_Integration_ShouldDeleteNewEmployee()
    {

        using (var cxt = new MMDatabaseContext(new DbContextOptions<MMDatabaseContext>(), false))
        {
            // Arrange
            var service = new EmployeeService(cxt);
            var controller = new EmployeeController(service);
            Employee testItem = new Employee()
            {

                FirstName = "integration",
                LastName = "b",
                Email = "dd",
                PhoneNumber = "1234567898"

            };

            // Act
            var result = (OkObjectResult)controller.Post(testItem).Result;
            var item = (Employee)result.Value;

            // Assert
            Assert.Equal("integration", item.FirstName);


        }

    }

    internal void Setup()
    {
        //mock IN-MEMORY context with data
        var context = new MMDatabaseContext(new DbContextOptions<MMDatabaseContext>());
        context.Employees = GetContextWithData().Employees;

        var interfaceEmployeeServiceMoq = new EmployeeService(context);
        _controller = new EmployeeController(interfaceEmployeeServiceMoq);

        curDatabaseType = "InMemory";
    }

    private MMDatabaseContext GetContextWithData()
    {
        var options = new DbContextOptionsBuilder<MMDatabaseContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;
        
        context = new MMDatabaseContext(options);
        context.Employees.Add(new Employee { id = 1, FirstName="a", LastName="b", Email="dd", PhoneNumber="1234567898"  });
        context.Employees.Add(new Employee { id = 2, FirstName = "a", LastName = "b", Email = "dd", PhoneNumber = "1234567898" });
        context.Employees.Add(new Employee { id = 3, FirstName = "a", LastName = "b", Email = "dd", PhoneNumber = "1234567898" });
        context.SaveChanges();

        return context;
    }
}