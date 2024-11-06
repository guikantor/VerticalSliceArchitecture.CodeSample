using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.CodeSample.API.Domain;

namespace VerticalSliceArchitecture.CodeSample.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasMany(p => p.Employees)
                                        .WithOne(p => p.Company)
                                        .HasForeignKey(p => p.CompanyId)
                                        .HasPrincipalKey(p => p.Id)
                                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>().HasKey(p => p.Id);
        }

    }
}