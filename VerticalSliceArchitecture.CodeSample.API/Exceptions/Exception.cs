namespace VerticalSliceArchitecture.CodeSample.API.Exceptions
{
    public class CompanyNotFoundException(Guid companyId) :
        Exception($"Company with id: {companyId} not found.")
    { }

    public class EmployeeNotFoundException(Guid employeeId) :
        Exception($"Employee with id: {employeeId} not found.")
    { }

}
