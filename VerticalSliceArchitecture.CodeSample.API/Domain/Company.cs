using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VerticalSliceArchitecture.CodeSample.API.Domain
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual ICollection<Employee> Employees { get; set; } = [];
    }
}
