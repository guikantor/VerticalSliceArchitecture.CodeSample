using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.CodeSample.API.Data;
using VerticalSliceArchitecture.CodeSample.API.Domain;
using VerticalSliceArchitecture.CodeSample.API.Exceptions;

namespace VerticalSliceArchitecture.CodeSample.API.Features.Employees.Commands
{

    public class CreateEmployeeCommand : IRequest<Employee>
    {
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
    }

    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Employee>
    {
        private readonly ApplicationDbContext _dbContext;
        public CreateEmployeeCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Employee> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var company = await _dbContext.Companies.FirstOrDefaultAsync(p => p.Id == request.CompanyId, cancellationToken)
                        ?? throw new CompanyNotFoundException(request.CompanyId);

            var Employee = new Employee()
            {
                Name = request.Name,
                Position = request.Position,
                Company = company,
                CompanyId = company.Id
            };

            var result = await _dbContext.Employees.AddAsync(Employee, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result.Entity;
        }
    }
}
