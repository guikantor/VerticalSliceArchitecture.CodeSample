using VerticalSliceArchitecture.CodeSample.API.Domain;
using VerticalSliceArchitecture.CodeSample.API.Exceptions;
using VerticalSliceArchitecture.CodeSample.API.Features.Employees.Queries;
using VerticalSliceArchitecture.CodeSample.Tests.Data;

namespace VerticalSliceArchitecture.CodeSample.Tests.Features.Employees.Queries
{
    public class GetEmployeeQueryTest
    {
        [Fact]
        public async Task Handle_GetEmployee_ReturnsEmployee()
        {
            // Arrange
            var firstCompany = new Company { Id = Guid.NewGuid(), Name = "FirstCompany" };
            var secondCompany = new Company { Id = Guid.NewGuid(), Name = "SecondCompany" };

            var firstEmployee = new Employee { Id = Guid.NewGuid(), CompanyId = firstCompany.Id, Company = firstCompany, Name = "TestEmployee", Position = "TestPosition" };
            var secondEmployee = new Employee { Id = Guid.NewGuid(), CompanyId = secondCompany.Id, Company = secondCompany, Name = "New TestEmployee", Position = "New TestPosition" };

            var dbContext = MemoryDbContext.GetDbContext([firstCompany, secondCompany], [firstEmployee, secondEmployee]);
            var queryHandler = new GetEmployeeQueryHandler(dbContext);
            var query = new GetEmployeeQuery { Id = secondEmployee.Id };

            // Act
            var result = await queryHandler.Handle(query, new CancellationToken());

            // Assert
            Assert.Equivalent(result, secondEmployee);
        }

        [Fact]
        public async Task Handle_EmployeeDoesNotExist_ThrowsEmployeeNotFoundException()
        {
            // Arrange
            var firstCompany = new Company { Id = Guid.NewGuid(), Name = "FirstCompany" };
            var secondCompany = new Company { Id = Guid.NewGuid(), Name = "SecondCompany" };

            var employee = new Employee { Id = Guid.NewGuid(), CompanyId = firstCompany.Id, Company = firstCompany, Name = "TestEmployee" };

            var dbContext = MemoryDbContext.GetDbContext([firstCompany, secondCompany], [employee]);
            var queryHandler = new GetEmployeeQueryHandler(dbContext);
            var query = new GetEmployeeQuery { Id = Guid.NewGuid() };

            // Act & Assert
            await Assert.ThrowsAsync<EmployeeNotFoundException>(() => queryHandler.Handle(query, new CancellationToken()));
        }

    }
}
