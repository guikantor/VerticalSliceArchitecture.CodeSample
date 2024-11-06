using VerticalSliceArchitecture.CodeSample.API.Domain;
using VerticalSliceArchitecture.CodeSample.API.Exceptions;
using VerticalSliceArchitecture.CodeSample.API.Features.Companies.Queries;
using VerticalSliceArchitecture.CodeSample.Tests.Data;

namespace VerticalSliceArchitecture.CodeSample.Tests.Features.Companies.Queries
{
    public class GetCompanyQueryTest
    {
        [Fact]
        public async Task Handle_GetCompany_ReturnsCompany()
        {
            // Arrange
            var firstCompany = new Company { Id = Guid.NewGuid(), Name = "TestCompany" };
            var secondCompany = new Company { Id = Guid.NewGuid(), Name = "New TestCompany" };

            var dbContext = MemoryDbContext.GetDbContext([firstCompany, secondCompany]);
            var queryHandler = new GetCompanyQueryHandler(dbContext);
            var query = new GetCompanyQuery { Id = firstCompany.Id };

            // Act
            var result = await queryHandler.Handle(query, new CancellationToken());

            // Assert
            Assert.Equivalent(result, firstCompany);
        }

        [Fact]
        public async Task Handle_CompanyDoesNotExist_ThrowsCompanyNotFoundException()
        {
            // Arrange
            var firstCompany = new Company { Id = Guid.NewGuid(), Name = "TestCompany" };
            var secondCompany = new Company { Id = Guid.NewGuid(), Name = "New TestCompany" };

            var dbContext = MemoryDbContext.GetDbContext([secondCompany]);
            var queryHandler = new GetCompanyQueryHandler(dbContext);
            var query = new GetCompanyQuery { Id = firstCompany.Id };

            // Act & Assert
            await Assert.ThrowsAsync<CompanyNotFoundException>(() => queryHandler.Handle(query, new CancellationToken()));
        }
    }
}
