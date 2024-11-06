using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.CodeSample.API.Data;
using VerticalSliceArchitecture.CodeSample.API.Domain;
using VerticalSliceArchitecture.CodeSample.API.Exceptions;

namespace VerticalSliceArchitecture.CodeSample.API.Features.Companies.Queries
{
    public class GetCompanyQuery : IRequest<Company>
    {
        public Guid Id { get; set; }
    }

    public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, Company>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetCompanyQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Company> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
        {
            var item = await _dbContext.Companies
                                        .Include(c => c.Employees)
                                        .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
                                        ?? throw new CompanyNotFoundException(request.Id);

            return item;
        }
    }
}
