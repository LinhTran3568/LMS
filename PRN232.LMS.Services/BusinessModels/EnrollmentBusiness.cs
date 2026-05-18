namespace PRN232.LMS.Services.BusinessModels;

public class EnrollmentBusiness
{
    public int EnrollmentId { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public StudentSummaryBusiness? Student { get; set; }
    public CourseWithSemesterBusiness? Course { get; set; }
}

public class StudentSummaryBusiness
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}

public class CourseWithSemesterBusiness
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int SemesterId { get; set; }
    public SemesterSummaryBusiness? Semester { get; set; }
}
