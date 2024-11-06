using VerticalSliceArchitecture.CodeSample.API.Domain;
using VerticalSliceArchitecture.CodeSample.API.Features.Companies.Commands;
using VerticalSliceArchitecture.CodeSample.Tests.Data;

namespace VerticalSliceArchitecture.CodeSample.Tests.Features.Companies.Commands
{
    public class CreateCompanyCommandTest
    {
        [Fact]
        public async Task Handle_CreateCompany_ReturnsCompany()
        {
            // Arrange
            var company = new Company { Name = "TestCompany" };

            var dbContext = MemoryDbContext.GetDbContext([]);
            var commandHandler = new CreateCompanyCommandHandler(dbContext);
            var command = new CreateCompanyCommand { Name = company.Name };

            // Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.NotEmpty(dbContext.Companies);
            Assert.NotNull(dbContext.Companies.First(p => p.Name == company.Name));
        }
    }
}