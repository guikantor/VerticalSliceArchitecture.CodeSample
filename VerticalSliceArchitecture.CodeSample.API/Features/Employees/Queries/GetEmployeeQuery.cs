using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.CodeSample.API.Data;
using VerticalSliceArchitecture.CodeSample.API.Domain;
using VerticalSliceArchitecture.CodeSample.API.Exceptions;

namespace VerticalSliceArchitecture.CodeSample.API.Features.Employees.Queries
{
    public class GetEmployeeQuery : IRequest<Employee>
    {
        public Guid Id { get; set; }
    }

    public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, Employee>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetEmployeeQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Employee> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            var item = await _dbContext.Employees
                                            .Include(g => g.Company)
                                            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
                                            ?? throw new EmployeeNotFoundException(request.Id);

            return item;
        }
    }
}
