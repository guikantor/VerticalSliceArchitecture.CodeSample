using VerticalSliceArchitecture.CodeSample.API.Domain;
using VerticalSliceArchitecture.CodeSample.API.Exceptions;
using VerticalSliceArchitecture.CodeSample.API.Features.Employees.Commands;
using VerticalSliceArchitecture.CodeSample.Tests.Data;

namespace VerticalSliceArchitecture.CodeSample.Tests.Features.Employees.Commands
{
    public class DeleteEmployeeCommandTest
    {
        [Fact]
        public async Task Handle_EmployeeExists_ReturnsTrue()
        {
            // Arrange
            var company = new Company { Id = new Guid() };
            var employee = new Employee { Id = Guid.NewGuid(), CompanyId = company.Id };

            var dbContext = MemoryDbContext.GetDbContext([company], [employee]);
            var commandHandler = new DeleteEmployeeCommandHandler(dbContext);
            var command = new DeleteEmployeeCommand { Id = employee.Id };

            // Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.True(result);
            var deletedEmployee = await dbContext.Employees.FindAsync(employee.Id);
            Assert.Null(deletedEmployee);
        }

        [Fact]
        public async Task Handle_EmployeeDoesNotExist_ThrowsEmployeeNotFoundException()
        {
            // Arrange
            var company = new Company { Id = new Guid() };
            var employee = new Employee { Id = Guid.NewGuid(), CompanyId = company.Id };
            var dbContext = MemoryDbContext.GetDbContext([company], [employee]);
            var employeeId = Guid.NewGuid();

            var commandHandler = new DeleteEmployeeCommandHandler(dbContext);
            var command = new DeleteEmployeeCommand { Id = employeeId };

            // Act & Assert
            await Assert.ThrowsAsync<EmployeeNotFoundException>(() => commandHandler.Handle(command, new CancellationToken()));
        }
    }
}
