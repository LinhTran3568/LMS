namespace PRN232.LMS.Repositories.Entities;

public class EnrollmentEntity
{
    public int EnrollmentId { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollDate { get; set; }
    public string Status { get; set; } = string.Empty;

    public StudentEntity? Student { get; set; }
    public CourseEntity? Course { get; set; }
}
