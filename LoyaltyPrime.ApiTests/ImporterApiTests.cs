using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.Services.Contexts.ImporterServices.Commands;
using LoyaltyPrime.Services.Contexts.ImporterServices.Models;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.WebApi.Controllers;
using MediatR;
using Moq;
using Xunit;

namespace LoyaltyPrime.ApiTests
{
    public class ImporterApiTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();

        [Fact]
        public async Task ImportFromJson_ShouldCreateMemberAndAccountAndCompany_OnSuccess()
        {
            //Arrange
            var importModel = CreateImportObjectSet();
            
            var expectedResult = ResultModel<Unit>.Success(200, "Import completed", Unit.Value);
            
            _mediatorMock.Setup(s =>
                    s.Send(It.IsAny<ImporterCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult)
                .Verifiable();

            ImporterController sut = new ImporterController(_mediatorMock.Object);

            //Act
            await sut.ImportFromJson(importModel);

            //Assert

            _mediatorMock.Verify(s =>
                s.Send(It.IsAny<ImporterCommand>(), It.IsAny<CancellationToken>()));
        }

        public List<ImportModel> CreateImportObjectSet()
        {
            return new List<ImportModel>()
            {
                new ImportModel("Anakin Skywalker", "Landsberger Straße 110", new List<ImportAccountModel>
                {
                    new ImportAccountModel("Burger King", 10, "ACTIVE"),
                    new ImportAccountModel("Fitness First", 150, "INACTIVE")
                }),
                new ImportModel("Yoda", "Landsberger Straße 125", new List<ImportAccountModel>()),
                new ImportModel("Obi-Wan Kenobi", "Landsberger Straße 114", new List<ImportAccountModel>
                {
                    new ImportAccountModel("Burger King", 20, "ACTIVE"),
                    new ImportAccountModel("Fitness First", 17, "ACTIVE"),
                    new ImportAccountModel("Lufthansa", 0, "ACTIVE")
                }),
            };
        }
    }
}