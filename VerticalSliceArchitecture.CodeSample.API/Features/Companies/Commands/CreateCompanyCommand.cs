using MediatR;
using VerticalSliceArchitecture.CodeSample.API.Data;
using VerticalSliceArchitecture.CodeSample.API.Domain;

namespace VerticalSliceArchitecture.CodeSample.API.Features.Companies.Commands
{
    public class CreateCompanyCommand : IRequest<Company>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Company>
    {
        private readonly ApplicationDbContext _dbContext;
        public CreateCompanyCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Company> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = new Company()
            {
                Name = request.Name
            };

            var result = await _dbContext.Companies.AddAsync(company, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result.Entity;
        }
    }
}
