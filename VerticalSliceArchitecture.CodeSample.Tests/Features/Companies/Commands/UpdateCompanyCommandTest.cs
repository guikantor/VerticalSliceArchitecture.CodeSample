using VerticalSliceArchitecture.CodeSample.API.Domain;
using VerticalSliceArchitecture.CodeSample.API.Exceptions;
using VerticalSliceArchitecture.CodeSample.API.Features.Companies.Commands;
using VerticalSliceArchitecture.CodeSample.Tests.Data;

namespace VerticalSliceArchitecture.CodeSample.Tests.Features.Companies.Commands
{
    public class UpdateCompanyCommandTest
    {

        [Fact]
        public async Task Handle_CompanyUpdate_ReturnsCompany()
        {
            // Arrange
            var company = new Company { Id = Guid.NewGuid(), Name = "TestCompany" };
            var newCompany = new Company { Id = company.Id, Name = "New TestCompany" };

            var dbContext = MemoryDbContext.GetDbContext([company]);
            var commandHandler = new UpdateCompanyCommandHandler(dbContext);
            var command = new UpdateCompanyCommand { Id = company.Id, Name = newCompany.Name };

            // Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.Equivalent(result, newCompany);
        }

        [Fact]
        public async Task Handle_CompanyDoesNotExist_ThrowsCompanyNotFoundException()
        {
            // Arrange
            var company = new Company { Id = Guid.NewGuid(), Name = "TestCompany" };
            var newCompany = new Company { Id = company.Id, Name = "New TestCompany" };

            var dbContext = MemoryDbContext.GetDbContext([]);
            var commandHandler = new UpdateCompanyCommandHandler(dbContext);
            var command = new UpdateCompanyCommand { Id = company.Id, Name = newCompany.Name };

            // Act & Assert
            await Assert.ThrowsAsync<CompanyNotFoundException>(() => commandHandler.Handle(command, new CancellationToken()));
        }
    }
}
