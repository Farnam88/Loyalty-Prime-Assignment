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

        [Fact]
        public async Task GetByIdAsync_ShouldFindEntity()
        {
            //Arrange
            var company = EntityGenerator.CreateCompany();
            var dbSetMock = new Mock<DbSet<Company>>();

            _contextMock.Setup(s => s.Set<Company>()).Returns(dbSetMock.Object);

            dbSetMock.Setup(s =>
                    s.FindAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

            _sut = new Repository<Company>(_contextMock.Object);

            //Act
            await _sut.GetByIdAsync(1);

            //Assert
            _contextMock.Verify(v => v.Set<Company>());
            dbSetMock.Verify(x => x.FindAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));
        }

        
        [Fact]
        //todo: having trouble with IAsyncQueryProvider

        public void CountAsync_ShouldReturnInt_IfExist()
        {
            //Arrange

            // var companies = EntityGenerator.CreateCompanies();
            //
            // var query = companies.AsQueryable();
            //
            // var dbSetMock = new Mock<DbSet<Company>>();
            //
            // dbSetMock.As<IAsyncEnumerable<Company>>()
            //     .Setup(s => s.GetAsyncEnumerator(default))
            //     .Returns(new MockAsyncEnumerator<Company>(query.GetEnumerator()));
            //
            // dbSetMock.As<IQueryable<Company>>()
            //     .Setup(s => s.Provider)
            //     .Returns(new MockAsyncQueryProvider<Company>(query.Provider));
            //
            // dbSetMock.As<IQueryable<Company>>()
            //     .Setup(s => s.Expression)
            //     .Returns(query.Expression);
            //
            // dbSetMock.As<IQueryable<Company>>()
            //     .Setup(s => s.ElementType)
            //     .Returns(query.ElementType);
            //
            // dbSetMock.As<IQueryable<Company>>()
            //     .Setup(s => s.GetEnumerator())
            //     .Returns(query.GetEnumerator);
            // _contextMock.Setup(s => s.Set<Company>())
            //     .Returns(dbSetMock.Object);
            //
            // _sut = new Repository<Company>(_contextMock.Object);

            //Act

            // var result = await _sut.CountAsync(p => p.Id > 0);

            //Assert
            // _contextMock.Verify(v => v.Set<Company>());
            // Assert.Equal(2, 2);
        }

        [Fact]
        //todo: having trouble with IAsyncQueryProvider
        public void FirstOrDefault_ShouldReturnEntity_IfFound()
        {
        }

        [Fact]
        //todo: having trouble with IAsyncQueryProvider
        public void FirstOrDefault_ShouldReturnDto_IfFound()
        {
        }

        [Fact]
        //todo: having trouble with IAsyncQueryProvider
        public void GetAllAsync_ShouldReturnDtoSet_IfFound()
        {
        }

        [Fact]
        //todo: having trouble with IAsyncQueryProvider
        public async Task GetAllAsync_ShouldReturnEntitySet_OnSuccess()
        {
            // //Arrange
            // var company = CreateCompany();
            // List<Company> companies = new List<Company>()
            // {
            //     company
            // };
            //
            // CompanySpecification specification = new CompanySpecification(p => p.Id > 0);
            // var queryable = companies.AsQueryable();
            // queryable = specification.Includes
            //     .Aggregate(queryable,
            //         (current, include) => current.Include(include))
            //     .Where(specification.Criteria);
            //
            // var dbSetMock = new Mock<DbSet<Company>>();
            //
            // dbSetMock.As<IAsyncEnumerable<Company>>()
            //     .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            //     .Returns(new TestAsyncEnumerator<Company>(queryable.GetEnumerator()));
            //
            // dbSetMock.As<IAsyncQueryProvider>().Setup(s => s.CreateQuery(queryable.Expression)).Returns(queryable);
            //
            // dbSetMock.As<IQueryable<Company>>()
            //     .Setup(m => m.Provider)
            //     .Returns(new TestAsyncQueryProvider<Company>(queryable.Provider));
            //
            // dbSetMock.As<IQueryable<Company>>().Setup(x => x.Expression).Returns(queryable.AsQueryable().Expression);
            // dbSetMock.As<IQueryable<Company>>().Setup(x => x.ElementType).Returns(queryable.AsQueryable().ElementType);
            // dbSetMock.As<IQueryable<Company>>().Setup(x => x.GetEnumerator())
            //     .Returns(() => queryable.GetEnumerator());
            //
            // _contextMock.Setup(s => s.Set<Company>()).Returns(dbSetMock.Object);
            //
            // _sut = new Repository<Company>(_contextMock.Object);
            //
            // //Act
            // var result = await _sut.GetAllAsync(specification);
            //
            // //Assert
            // Assert.Null(result);
        }
    }
}