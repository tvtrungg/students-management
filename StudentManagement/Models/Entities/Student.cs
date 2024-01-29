namespace StudentManagement.Models.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public Scores? Scores { get; set; }
    }
}
