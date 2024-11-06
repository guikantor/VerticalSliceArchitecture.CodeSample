using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.CodeSample.API.Data;
using VerticalSliceArchitecture.CodeSample.API.Domain;
using VerticalSliceArchitecture.CodeSample.API.Exceptions;

namespace VerticalSliceArchitecture.CodeSample.API.Features.Companies.Commands
{
    public class UpdateCompanyCommand : IRequest<Company>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Company>
    {
        private readonly ApplicationDbContext _dbContext;
        public UpdateCompanyCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Company> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _dbContext.Companies.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken) ??
                       throw new CompanyNotFoundException(request.Id);


            company.Name = request.Name;

            var result = _dbContext.Companies.Update(company);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result.Entity;
        }
    }
}
