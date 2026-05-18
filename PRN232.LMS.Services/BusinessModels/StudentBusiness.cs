namespace PRN232.LMS.Services.BusinessModels;

public class StudentBusiness
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public List<StudentEnrollmentBusiness>? Enrollments { get; set; }
}

public class StudentEnrollmentBusiness
{
    public int EnrollmentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public CourseSummaryBusiness? Course { get; set; }
}
