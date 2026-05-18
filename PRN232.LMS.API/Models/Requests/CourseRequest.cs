namespace PRN232.LMS.API.Models.Requests;

public class CourseRequest
{
    public string CourseName { get; set; } = string.Empty;
    public int SemesterId { get; set; }
}
