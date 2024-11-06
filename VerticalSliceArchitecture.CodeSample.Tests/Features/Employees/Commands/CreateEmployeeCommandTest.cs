using VerticalSliceArchitecture.CodeSample.API.Domain;
using VerticalSliceArchitecture.CodeSample.API.Exceptions;
using VerticalSliceArchitecture.CodeSample.API.Features.Employees.Commands;
using VerticalSliceArchitecture.CodeSample.Tests.Data;

namespace VerticalSliceArchitecture.CodeSample.Tests.Features.Employees.Commands
{
    public class CreateEmployeeCommandTest
    {
        [Fact]
        public async Task Handle_CreateEmployee_ReturnsEmployee()
        {
            // Arrange
            var company = new Company { Id = Guid.NewGuid(), Name = "TestCompany" };
            var employee = new Employee { CompanyId = company.Id, Name = "TestEmployee", Position = "TestPosition" };
            var dbContext = MemoryDbContext.GetDbContext([company]);

            var commandHandler = new CreateEmployeeCommandHandler(dbContext);
            var command = new CreateEmployeeCommand { Name = employee.Name, Position = employee.Position, CompanyId = employee.CompanyId };

            // Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.NotEmpty(dbContext.Employees);
            Assert.NotNull(dbContext.Employees.First(p => p.Name == employee.Name));
        }

        [Fact]
        public async Task Handle_CompanyDoesNotExist_ThrowsCompanyNotFoundException()
        {
            // Arrange
            var company = new Company { Id = Guid.NewGuid(), Name = "TestCompany" };
            var employee = new Employee { CompanyId = Guid.NewGuid(), Name = "TestEmployee", Position = "TestPosition" };

            var dbContext = MemoryDbContext.GetDbContext([company]);
            var commandHandler = new CreateEmployeeCommandHandler(dbContext);
            var command = new CreateEmployeeCommand { Name = employee.Name, CompanyId = employee.CompanyId };

            // Act & Assert
            await Assert.ThrowsAsync<CompanyNotFoundException>(() => commandHandler.Handle(command, new CancellationToken()));
        }
    }
}
