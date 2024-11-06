using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.CodeSample.API.Data;
using VerticalSliceArchitecture.CodeSample.API.Exceptions;

namespace VerticalSliceArchitecture.CodeSample.API.Features.Companies.Commands
{
    public class DeleteCompanyCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, bool>
    {
        private readonly ApplicationDbContext _dbContext;
        public DeleteCompanyCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _dbContext.Companies.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken)
                ?? throw new CompanyNotFoundException(request.Id);

            _dbContext.Companies.Remove(company);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

    }
}