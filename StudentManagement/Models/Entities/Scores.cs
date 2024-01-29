namespace StudentManagement.Models.Entities
{
    public class Scores
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        public float? Score { get; set; }
    }
}