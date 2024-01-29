namespace StudentManagement.Models.Entities
{
    public class StudentScore
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public List<ScoreInfo> Scores { get; set; }
    }

    public class ScoreInfo
    {
        public Guid SubjectId { get; set; }
        public float? Score { get; set; }
    }
}
