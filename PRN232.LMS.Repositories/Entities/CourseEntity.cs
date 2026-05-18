namespace PRN232.LMS.Repositories.Entities;

public class CourseEntity
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int SemesterId { get; set; }

    public SemesterEntity? Semester { get; set; }
    public ICollection<EnrollmentEntity> Enrollments { get; set; } = new List<EnrollmentEntity>();
}
