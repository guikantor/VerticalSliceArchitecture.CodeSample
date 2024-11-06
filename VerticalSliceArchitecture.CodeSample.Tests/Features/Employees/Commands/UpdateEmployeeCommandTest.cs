using VerticalSliceArchitecture.CodeSample.API.Domain;
using VerticalSliceArchitecture.CodeSample.API.Exceptions;
using VerticalSliceArchitecture.CodeSample.API.Features.Employees.Commands;
using VerticalSliceArchitecture.CodeSample.Tests.Data;

namespace VerticalSliceArchitecture.CodeSample.Tests.Features.Employees.Commands
{
    public class UpdateEmployeeCommandTest
    {
        [Fact]
        public async Task Handle_EmployeeUpdate_ReturnsEmployee()
        {
            // Arrange
            var firstCompany = new Company { Id = Guid.NewGuid(), Name = "FirstCompany" };
            var secondCompany = new Company { Id = Guid.NewGuid(), Name = "SecondCompany" };

            var employee = new Employee { Id = Guid.NewGuid(), CompanyId = firstCompany.Id, Company = firstCompany, Name = "TestEmployee", Position = "TestPosition" };
            var newEmployee = new Employee { Id = employee.Id, CompanyId = secondCompany.Id, Company = secondCompany, Name = "New TestEmployee", Position = "New TestPosition" };

            var dbContext = MemoryDbContext.GetDbContext([firstCompany, secondCompany], [employee]);
            var commandHandler = new UpdateEmployeeCommandHandler(dbContext);
            var command = new UpdateEmployeeCommand { Id = newEmployee.Id, Name = newEmployee.Name, CompanyId = newEmployee.CompanyId, Position = newEmployee.Position };

            // Act
            var result = await commandHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.Equivalent(result, newEmployee);
        }

        [Fact]
        public async Task Handle_EmployeeDoesNotExist_ThrowsEmployeeNotFoundException()
        {
            // Arrange
            var firstCompany = new Company { Id = Guid.NewGuid(), Name = "FirstCompany" };
            var secondCompany = new Company { Id = Guid.NewGuid(), Name = "SecondCompany" };
            var employee = new Employee { Id = Guid.NewGuid(), CompanyId = firstCompany.Id, Company = firstCompany, Name = "TestEmployee", Position = "TestPosition" };

            var dbContext = MemoryDbContext.GetDbContext([firstCompany, secondCompany], [employee]);
            var commandHandler = new UpdateEmployeeCommandHandler(dbContext);
            var command = new UpdateEmployeeCommand { Id = Guid.NewGuid(), Name = employee.Name, CompanyId = employee.CompanyId, Position = employee.Position };

            // Act & Assert
            await Assert.ThrowsAsync<EmployeeNotFoundException>(() => commandHandler.Handle(command, new CancellationToken()));
        }


        [Fact]
        public async Task Handle_CompanyDoesNotExist_ThrowsCompanyNotFoundException()
        {
            // Arrange
            var firstCompany = new Company { Id = Guid.NewGuid(), Name = "FirstCompany" };
            var secondCompany = new Company { Id = Guid.NewGuid(), Name = "SecondCompany" };

            var employee = new Employee { Id = Guid.NewGuid(), CompanyId = firstCompany.Id, Company = firstCompany, Name = "TestEmployee", Position = "TestPosition" };

            var dbContext = MemoryDbContext.GetDbContext([firstCompany], [employee]);
            var commandHandler = new UpdateEmployeeCommandHandler(dbContext);
            var command = new UpdateEmployeeCommand { Id = employee.Id, Name = employee.Name, CompanyId = secondCompany.Id, Position = employee.Position };

            // Act & Assert
            await Assert.ThrowsAsync<CompanyNotFoundException>(() => commandHandler.Handle(command, new CancellationToken()));
        }
    }
}
