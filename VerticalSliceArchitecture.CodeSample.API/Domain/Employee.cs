using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VerticalSliceArchitecture.CodeSample.API.Domain
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public Guid CompanyId { get; set; }

        [JsonIgnore]
        public virtual Company Company { get; set; } = new Company();

    }
}
