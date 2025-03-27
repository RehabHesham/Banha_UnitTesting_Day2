using CarAPI.Entities;
using CarFactoryAPI.Entities;
using CarFactoryAPI.Repositories_DAL;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFactoryAPI_Tests
{
    public class OwnerRepositoryTests
    {
        Mock<FactoryContext> factoryContextMock;
        OwnerRepository ownerRepo;
        public OwnerRepositoryTests()
        {
            factoryContextMock = new();

            ownerRepo = new(factoryContextMock.Object);
        }
        [Fact]
        public void GetOwnerById_id10_returnObject()
        {
            // Arrange
            // prepare mocking data
            List<Owner> owners = new()
            {
                new Owner { Id = 1, Name = "Ali"},
                new Owner { Id = 2, Name = "Ahmed"},
                new Owner { Id = 3, Name = "Omar"},
            };

            // setup mocking Methods
            factoryContextMock.Setup(o => o.Owners).ReturnsDbSet(owners);

            // Act
            Owner owner = ownerRepo.GetOwnerById(1);

            // Assert
            Assert.Equal("Ali", owner.Name);
        }
    }
}
