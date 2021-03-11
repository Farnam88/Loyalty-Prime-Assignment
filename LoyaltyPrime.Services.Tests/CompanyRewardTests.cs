using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.CompanyRewardServices.Commands;
using LoyaltyPrime.Services.Contexts.CompanyRewardServices.Dto;
using LoyaltyPrime.Services.Contexts.CompanyRewardServices.Queries;
using Moq;
using Xunit;

namespace LoyaltyPrime.Services.Tests
{
    public class CompanyRewardTests
    {
        private readonly Mock<IRepository<CompanyReward>> _companyRewardRepositoryMock =
            new Mock<IRepository<CompanyReward>>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        [Fact]
        public async Task CreateCompanyReward_ShouldCreateReward_IfCompanyExists()
        {
            //Arrange
            var company = new Company("Burger King") {Id = 1};
            var rewardOption = new CompanyReward("Free Cofee", company.Id, 50) {Id = 1};
            Mock<IRepository<Company>> companyRepositoryMock = new Mock<IRepository<Company>>();

            companyRepositoryMock.Setup(s =>
                    s.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company).Verifiable();

            _companyRewardRepositoryMock.Setup(s =>
                    s.AddAsync(rewardOption, It.IsAny<CancellationToken>()))
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CompanyRewardRepository)
                .Returns(_companyRewardRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CompanyRepository)
                .Returns(companyRepositoryMock.Object)
                .Verifiable();

            CreateCompanyRewardCommand command = new CreateCompanyRewardCommand(rewardOption.CompanyId,
                rewardOption.RewardTitle, rewardOption.RewardPoints);

            CreateCompanyRewardCommandHandler sut =
                new CreateCompanyRewardCommandHandler(_unitOfWorkMock.Object);

            //Act
            var result = await sut.Handle(command, It.IsAny<CancellationToken>());

            //Assert
            _unitOfWorkMock.Verify(v => v.CompanyRewardRepository);

            _unitOfWorkMock.Verify(v => v.CompanyRepository);

            companyRepositoryMock.Verify(v =>
                v.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));

            _companyRewardRepositoryMock.Verify(v =>
                v.AddAsync(It.IsAny<CompanyReward>(), It.IsAny<CancellationToken>()));

            _unitOfWorkMock.Verify(v => v.CommitAsync(It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded && result.StatusCode == 201);
        }


        [Fact]
        public async Task GetMemberActiveAccounts_ShouldReturnSetOfActiveAccount_IfMemberAndAccountExists()
        {
            //Arrange
            var company = new Company("Farnam") {Id = 1};
            List<CompanyRewardDto> companyRewardDto = CompanyRewardsDto(company.Id, company.Name);

            _companyRewardRepositoryMock.Setup(s =>
                    s.GetAllAsync(It.IsAny<ISpecification<CompanyReward, CompanyRewardDto>>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyRewardDto)
                .Verifiable();

            Mock<IRepository<Company>> companyRepositoryMock = new Mock<IRepository<Company>>();
            companyRepositoryMock.Setup(s =>
                    s.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CompanyRepository).Returns(companyRepositoryMock.Object);
            _unitOfWorkMock.Setup(s => s.CompanyRewardRepository).Returns(_companyRewardRepositoryMock.Object);

            GetCompanyRewardsQuery query = new GetCompanyRewardsQuery(company.Id);

            GetCompanyRewardsQueryHandler sut = new GetCompanyRewardsQueryHandler(_unitOfWorkMock.Object);

            //Act

            var result = await sut.Handle(query, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.CompanyRepository);
            
            _unitOfWorkMock.Verify(v => v.CompanyRewardRepository);

            companyRepositoryMock.Verify(v =>
                v.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));

            _companyRewardRepositoryMock.Verify(v =>
                v.GetAllAsync(It.IsAny<ISpecification<CompanyReward, CompanyRewardDto>>(),
                    It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded && result.StatusCode == 200);

            Assert.NotNull(result.Result);
        }

        private List<CompanyRewardDto> CompanyRewardsDto(int companyId, string companyName)
        {
            return new List<CompanyRewardDto>
            {
                new CompanyRewardDto(companyId, companyName, "Free Cofee", 50)
            };
        }
    }
}