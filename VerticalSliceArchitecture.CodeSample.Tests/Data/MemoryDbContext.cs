using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.CodeSample.API.Data;
using VerticalSliceArchitecture.CodeSample.API.Domain;

namespace VerticalSliceArchitecture.CodeSample.Tests.Data
{
    public static class MemoryDbContext
    {
        public static ApplicationDbContext GetDbContext(List<Company> companies, List<Employee>? employees = null)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "DBMemoryTest").Options;

            var dbContext = new ApplicationDbContext(options);
            dbContext.Companies.AddRange(companies);

            if (employees is not null)
                dbContext.Employees.AddRange(employees);

            dbContext.SaveChanges();

            return dbContext;
        }
    }
}
