using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.CodeSample.API.Data;
using VerticalSliceArchitecture.CodeSample.API.Domain;
using VerticalSliceArchitecture.CodeSample.API.Exceptions;

namespace VerticalSliceArchitecture.CodeSample.API.Features.Employees.Commands
{
    public class UpdateEmployeeCommand : IRequest<Employee>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
    }

    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Employee>
    {
        private readonly ApplicationDbContext _dbContext;
        public UpdateEmployeeCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Employee> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken) ??
                        throw new EmployeeNotFoundException(request.Id);

            var company = await _dbContext.Companies.FirstOrDefaultAsync(p => p.Id == request.CompanyId, cancellationToken) ??
                        throw new CompanyNotFoundException(request.CompanyId);

            employee.Name = request.Name;
            employee.Position = request.Position;
            employee.Company = company;
            employee.CompanyId = request.CompanyId;

            var result = _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result.Entity;
        }
    }
}
