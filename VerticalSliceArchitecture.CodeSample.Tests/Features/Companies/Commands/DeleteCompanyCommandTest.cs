using VerticalSliceArchitecture.CodeSample.API.Domain;
using VerticalSliceArchitecture.CodeSample.API.Exceptions;
using VerticalSliceArchitecture.CodeSample.API.Features.Companies.Commands;
using VerticalSliceArchitecture.CodeSample.Tests.Data;

namespace VerticalSliceArchitecture.CodeSample.Tests.Features.Companies.Commands
{
    public class DeleteCompanyCommandTest
    {
        [Fact]
        public async Task Handle_CompanyExists_ReturnsTrue()
        {
            // Arrange
            var company = new Company { Id = Guid.NewGuid() };
            var dbContext = MemoryDbContext.GetDbContext([company]);
            var commandHandler = new DeleteCompanyCommandHandler(dbContext);
            var command = new DeleteCompanyCommand { Id = company.Id };

            // Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.True(result);
            var deletedCompany = await dbContext.Companies.FindAsync(company.Id);
            Assert.Null(deletedCompany);
        }

        [Fact]
        public async Task Handle_CompanyDoesNotExist_ThrowsCompanyNotFoundException()
        {
            // Arrange
            var dbContext = MemoryDbContext.GetDbContext([]);
            var companyId = Guid.NewGuid();

            var commandHandler = new DeleteCompanyCommandHandler(dbContext);
            var command = new DeleteCompanyCommand { Id = companyId };

            // Act & Assert
            await Assert.ThrowsAsync<CompanyNotFoundException>(() => commandHandler.Handle(command, new CancellationToken()));
        }
    }
}
