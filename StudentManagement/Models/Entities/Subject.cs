using System.Runtime.CompilerServices;

namespace StudentManagement.Models.Entities
{
    public class Subject
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
