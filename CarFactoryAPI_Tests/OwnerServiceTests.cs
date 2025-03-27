using CarAPI.Entities;
using CarAPI.Models;
using CarAPI.Payment;
using CarAPI.Repositories_DAL;
using CarAPI.Services_BLL;
using CarFactoryAPI.Entities;
using CarFactoryAPI.Repositories_DAL;
using CarFactoryAPI_Tests.Stups;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace CarFactoryAPI_Tests
{
    public class OwnerServiceTests : IDisposable
    {
        private readonly ITestOutputHelper outputHelper;
        // Create Mock For Dependencies
        Mock<ICarsRepository> carRepoMock;
        Mock<IOwnersRepository> ownerRepoMock;
        Mock<ICashService> cashServiceMock;

        // use mock object as a fake dependency
        OwnersService ownersService;

        
        public OwnerServiceTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
            outputHelper.WriteLine("test setup");

            // Create Mock For Dependencies
            carRepoMock = new();
            ownerRepoMock = new();
            cashServiceMock = new();

            // use mock object as a fake dependency
            ownersService = new(carRepoMock.Object, ownerRepoMock.Object, cashServiceMock.Object);

        }
        public void Dispose()
        {
            outputHelper.WriteLine("test clean up");
        }

        [Trait("Developer","Ali")]
        [Fact]
        public void BuyCar_CarId10_NotExist()
        {
            outputHelper.WriteLine("test car not exist");

            // Arrange
            FactoryContext factoryContext = new();

            CarRepository carRepo = new(factoryContext);
            OwnerRepository ownerRepo = new(factoryContext);
            CashService cashService = new();

            OwnersService ownersService = new(carRepo,ownerRepo,cashService);

            BuyCarInput buyCar = new BuyCarInput()
            {
                CarId = 10,
                OwnerId = 1,
                Amount = 1000
            };

            // Act
           string result = ownersService.BuyCar(buyCar);

            // Assert
            Assert.Contains("doesn't exist", result);
        }

        [Trait("Developer", "Omar")]
        [Fact]
        public void BuyCar_CarId15_CarWithOwner()
        {
            outputHelper.WriteLine("test car sold");

            // Arrange

            OwnersService ownersService = new(new CarRepoStup(), new OwnerRepoStup(), new CashService());
            BuyCarInput buyCarInput = new()
            {
                CarId = 15,
                OwnerId = 2,
                Amount = 1000
            };

            // Act
            string result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Contains("Already sold", result);
        }

        [Trait("Developer", "Ali")]
        [Fact]
        public void BuyCar_CarId5OwnerId10_NotExist()
        {
            outputHelper.WriteLine("test owner not exist");

            // Arrange

            // Create Mocking of Dependencies
            //Mock<ICarsRepository> carRepoMock = new();
            //Mock<IOwnersRepository> ownerRepoMock = new();
            //Mock<ICashService> cashServiceMock = new();

            // prepare Mocking Data
            Car car = new() { Id = 5, Price = 1000, Type = CarType.BMW, Velocity = 500, VIN = "24667687"};
            Owner owner = null;

            // Setup Mocking Methods
            carRepoMock.Setup(o=>o.GetCarById(It.IsAny<int>())).Returns(car);
            ownerRepoMock.Setup(o => o.GetOwnerById(It.IsAny<int>())).Returns(owner);

            //// use mock as a fake dependency
            //OwnersService ownersService = new(carRepoMock.Object, ownerRepoMock.Object, cashServiceMock.Object);

            BuyCarInput buyCarInput = new()
            {
                CarId = 5,
                OwnerId = 10,
                Amount = 1000
            };

            // Act
            string result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Contains("doesn't exist", result);
        }

        [Trait("Developer", "Ahmed")]
        [Fact]
        public void BuyCar_CarId5OwnerId2_AmountNotEnough()
        {
            outputHelper.WriteLine("test amount to pay not enough");

            // Arrange
           

            // Prepare Mocking Data
            Car car = new Car() { Id = 5, Type = CarType.BMW, Velocity = 200, Price = 2000, VIN = "45546" };
            Owner owner = new Owner() { Id = 2, Name = "Ali" };

            // setup mocking methods
            carRepoMock.Setup(o => o.GetCarById(It.IsAny<int>())).Returns(car);
            ownerRepoMock.Setup(o => o.GetOwnerById(It.IsAny<int>())).Returns(owner);

            
            BuyCarInput buyCarInput = new()
            {
                CarId = 5,
                OwnerId = 2,
                Amount = 1000
            };

            // Act
            string result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Equal("Insufficient funds", result);
        }

        
    }
}
