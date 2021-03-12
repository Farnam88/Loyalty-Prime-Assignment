using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer.Infrastructure.Repositories;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.DataLayer;
using LoyaltyPrime.Infrastructure.Tests.Helpers;
using LoyaltyPrime.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LoyaltyPrime.Infrastructure.Tests
{
    public class GenericRepositoryTests
    {
        private IRepository<Company> _sut;

        private readonly Mock<LoyaltyPrimeContext> _contextMock = new Mock<LoyaltyPrimeContext>();

        [Fact]
        public async Task AddAsync_ShouldAddEntity_OnSuccess()
        {
            //Arrange
            var company = EntityGenerator.CreateCompany();
            var dbSetMock = new Mock<DbSet<Company>>();

            _contextMock.Setup(s => s.Set<Company>())
                .Returns(dbSetMock.Object);

            dbSetMock.Setup(s =>
                s.AddAsync(company, It.IsAny<CancellationToken>()));

            _sut = new Repository<Company>(_contextMock.Object);


            //Act
            await _sut.AddAsync(company, It.IsAny<CancellationToken>());

            //Assert
            _contextMock.Verify(v => v.Set<Company>());

            dbSetMock.Verify(v =>
                v.AddAsync(It.Is<Company>(i =>
                    i == company), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public void Update_ShouldUpdateEntity_OnSuccess()
        {
            //Arrange
            var company = EntityGenerator.CreateCompany();
            company.Name = "new company name";
            var dbSetMock = new Mock<DbSet<Company>>();

            _contextMock.Setup(s => s.Set<Company>())
                .Returns(dbSetMock.Object);

            dbSetMock.Setup(s =>
                s.Update(company));
            _sut = new Repository<Company>(_contextMock.Object);


            //Act
            _sut.Update(company);

            //Assert
            _contextMock.Verify(v => v.Set<Company>());

            dbSetMock.Verify(v =>
                v.Update(It.Is<Company>(i =>
                    i == company)));
        }

        [Fact]
        public async Task AddRangeAsync_ShouldAddEntities_OnSuccess()
        {
            //Arrange
            var companies = EntityGenerator.CreateCompanies();
            var dbSetMock = new Mock<DbSet<Company>>();

            _contextMock.Setup(s => s.Set<Company>())
                .Returns(dbSetMock.Object);

            dbSetMock.Setup(s =>
                s.AddRangeAsync(companies, It.IsAny<CancellationToken>()));

            _sut = new Repository<Company>(_contextMock.Object);


            //Act
            await _sut.AddRangeAsync(companies, It.IsAny<CancellationToken>());

            //Assert
            _contextMock.Verify(v => v.Set<Company>());

            dbSetMock.Verify(v =>
                v.AddRangeAsync(It.Is<List<Company>>(i =>
                    Equals(i, companies)), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public void DeleteAsync_ShouldRemoveEntity_OnSuccess()
        {
            //Arrange
            var company = EntityGenerator.CreateCompany();
            var dbSetMock = new Mock<DbSet<Company>>();

            _contextMock.Setup(s => s.Set<Company>())
                .Returns(dbSetMock.Object);

            dbSetMock.Setup(s =>
                    s.FindAsync(It.IsAny<int>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            dbSetMock.Setup(s => s.Remove(It.IsAny<Company>()));

            _sut = new Repository<Company>(_contextMock.Object);

            //Act
            _sut.DeleteAsync(1);

            //Assert
            _contextMock.Verify(v => v.Set<Company>());

            dbSetMock.Verify(v =>
                v.FindAsync(It.IsAny<int>(),
                    It.IsAny<CancellationToken>()));

            dbSetMock.Verify(v =>
                v.Remove(It.Is<Company>(i => i == company)));
        }

        [Fact]
        public void Delete_ShouldRemoveEntity_OnSuccess()
        {
            //Arrange
            var company = EntityGenerator.CreateCompany();
            
            var dbSetMock = new Mock<DbSet<Company>>();
            
            _contextMock.Setup(s => s.Set<Company>())
                .Returns(dbSetMock.Object);

            dbSetMock.Setup(s => s.Remove(It.IsAny<Company>()));

            _sut = new Repository<Company>(_contextMock.Object);

            //Act
            _sut.Delete(company);

            //Assert
            _contextMock.Verify(v => v.Set<Company>());

            dbSetMock.Verify(v =>
                v.Remove(It.Is<Company>(i => i == company)));
        }
    }
}