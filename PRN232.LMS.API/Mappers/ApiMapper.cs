using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.API.Models.Responses;
using PRN232.LMS.Services.BusinessModels;

namespace PRN232.LMS.API.Mappers;

public static class ApiMapper
{
    public static SemesterResponse ToResponse(SemesterBusiness business) => new()
    {
        SemesterId = business.SemesterId,
        SemesterName = business.SemesterName,
        StartDate = business.StartDate,
        EndDate = business.EndDate,
        Courses = business.Courses?.Select(c => new CourseSummaryResponse
        {
            CourseId = c.CourseId,
            CourseName = c.CourseName,
            SemesterId = c.SemesterId
        }).ToList()
    };

    public static SemesterBusiness ToBusiness(SemesterRequest request) => new()
    {
        SemesterName = request.SemesterName,
        StartDate = request.StartDate,
        EndDate = request.EndDate
    };

    public static CourseResponse ToResponse(CourseBusiness business) => new()
    {
        CourseId = business.CourseId,
        CourseName = business.CourseName,
        SemesterId = business.SemesterId,
        Semester = business.Semester == null ? null : new SemesterSummaryResponse
        {
            SemesterId = business.Semester.SemesterId,
            SemesterName = business.Semester.SemesterName,
            StartDate = business.Semester.StartDate,
            EndDate = business.Semester.EndDate
        },
        Enrollments = business.Enrollments?.Select(e => new EnrollmentSummaryResponse
        {
            EnrollmentId = e.EnrollmentId,
            StudentId = e.StudentId,
            CourseId = e.CourseId,
            EnrollDate = e.EnrollDate,
            Status = e.Status
        }).ToList()
    };

    public static CourseBusiness ToBusiness(CourseRequest request) => new()
    {
        CourseName = request.CourseName,
        SemesterId = request.SemesterId
    };

    public static SubjectResponse ToResponse(SubjectBusiness business) => new()
    {
        SubjectId = business.SubjectId,
        SubjectCode = business.SubjectCode,
        SubjectName = business.SubjectName,
        Credit = business.Credit
    };

    public static SubjectBusiness ToBusiness(SubjectRequest request) => new()
    {
        SubjectCode = request.SubjectCode,
        SubjectName = request.SubjectName,
        Credit = request.Credit
    };

    public static StudentResponse ToResponse(StudentBusiness business) => new()
    {
        StudentId = business.StudentId,
        FullName = business.FullName,
        Email = business.Email,
        DateOfBirth = business.DateOfBirth,
        Enrollments = business.Enrollments?.Select(e => new StudentEnrollmentResponse
        {
            EnrollmentId = e.EnrollmentId,
            CourseId = e.CourseId,
            EnrollDate = e.EnrollDate,
            Status = e.Status,
            Course = e.Course == null ? null : new CourseSummaryResponse
            {
                CourseId = e.Course.CourseId,
                CourseName = e.Course.CourseName,
                SemesterId = e.Course.SemesterId
            }
        }).ToList()
    };

    public static StudentBusiness ToBusiness(StudentRequest request) => new()
    {
        FullName = request.FullName,
        Email = request.Email,
        DateOfBirth = request.DateOfBirth
    };

    public static EnrollmentResponse ToResponse(EnrollmentBusiness business) => new()
    {
        EnrollmentId = business.EnrollmentId,
        StudentId = business.StudentId,
        CourseId = business.CourseId,
        EnrollDate = business.EnrollDate,
        Status = business.Status,
        Student = business.Student == null ? null : new StudentSummaryResponse
        {
            StudentId = business.Student.StudentId,
            FullName = business.Student.FullName,
            Email = business.Student.Email,
            DateOfBirth = business.Student.DateOfBirth
        },
        Course = business.Course == null ? null : new CourseWithSemesterResponse
        {
            CourseId = business.Course.CourseId,
            CourseName = business.Course.CourseName,
            SemesterId = business.Course.SemesterId,
            Semester = business.Course.Semester == null ? null : new SemesterSummaryResponse
            {
                SemesterId = business.Course.Semester.SemesterId,
                SemesterName = business.Course.Semester.SemesterName,
                StartDate = business.Course.Semester.StartDate,
                EndDate = business.Course.Semester.EndDate
            }
        }
    };

    public static EnrollmentBusiness ToBusiness(EnrollmentRequest request) => new()
    {
        StudentId = request.StudentId,
        CourseId = request.CourseId,
        EnrollDate = request.EnrollDate,
        Status = request.Status
    };
}
