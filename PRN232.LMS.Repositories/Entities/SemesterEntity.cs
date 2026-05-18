namespace PRN232.LMS.Repositories.Entities;

public class SemesterEntity
{
    public int SemesterId { get; set; }
    public string SemesterName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ICollection<CourseEntity> Courses { get; set; } = new List<CourseEntity>();
}
