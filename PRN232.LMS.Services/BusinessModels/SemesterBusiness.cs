namespace PRN232.LMS.Services.BusinessModels;

public class SemesterBusiness
{
    public int SemesterId { get; set; }
    public string SemesterName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<CourseSummaryBusiness>? Courses { get; set; }
}

public class CourseSummaryBusiness
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int SemesterId { get; set; }
}
