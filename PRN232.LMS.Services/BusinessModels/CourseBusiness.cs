namespace PRN232.LMS.Services.BusinessModels;

public class CourseBusiness
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int SemesterId { get; set; }
    public SemesterSummaryBusiness? Semester { get; set; }
    public List<EnrollmentSummaryBusiness>? Enrollments { get; set; }
}

public class SemesterSummaryBusiness
{
    public int SemesterId { get; set; }
    public string SemesterName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class EnrollmentSummaryBusiness
{
    public int EnrollmentId { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
