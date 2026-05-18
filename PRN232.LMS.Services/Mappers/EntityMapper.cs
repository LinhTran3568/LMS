using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Services.BusinessModels;

namespace PRN232.LMS.Services.Mappers;

public static class EntityMapper
{
    public static SemesterBusiness ToBusiness(SemesterEntity entity, bool includeCourses)
    {
        var business = new SemesterBusiness
        {
            SemesterId = entity.SemesterId,
            SemesterName = entity.SemesterName,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate
        };

        if (includeCourses && entity.Courses.Count > 0)
        {
            business.Courses = entity.Courses.Select(c => new CourseSummaryBusiness
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                SemesterId = c.SemesterId
            }).ToList();
        }

        return business;
    }

    public static CourseBusiness ToBusiness(CourseEntity entity, bool includeSemester, bool includeEnrollments)
    {
        var business = new CourseBusiness
        {
            CourseId = entity.CourseId,
            CourseName = entity.CourseName,
            SemesterId = entity.SemesterId
        };

        if (includeSemester && entity.Semester != null)
        {
            business.Semester = ToSemesterSummary(entity.Semester);
        }

        if (includeEnrollments && entity.Enrollments.Count > 0)
        {
            business.Enrollments = entity.Enrollments.Select(e => new EnrollmentSummaryBusiness
            {
                EnrollmentId = e.EnrollmentId,
                StudentId = e.StudentId,
                CourseId = e.CourseId,
                EnrollDate = e.EnrollDate,
                Status = e.Status
            }).ToList();
        }

        return business;
    }

    public static SubjectBusiness ToBusiness(SubjectEntity entity) => new()
    {
        SubjectId = entity.SubjectId,
        SubjectCode = entity.SubjectCode,
        SubjectName = entity.SubjectName,
        Credit = entity.Credit
    };

    public static StudentBusiness ToBusiness(StudentEntity entity, bool includeEnrollments)
    {
        var business = new StudentBusiness
        {
            StudentId = entity.StudentId,
            FullName = entity.FullName,
            Email = entity.Email,
            DateOfBirth = entity.DateOfBirth
        };

        if (includeEnrollments && entity.Enrollments.Count > 0)
        {
            business.Enrollments = entity.Enrollments.Select(e => new StudentEnrollmentBusiness
            {
                EnrollmentId = e.EnrollmentId,
                CourseId = e.CourseId,
                EnrollDate = e.EnrollDate,
                Status = e.Status,
                Course = e.Course == null ? null : new CourseSummaryBusiness
                {
                    CourseId = e.Course.CourseId,
                    CourseName = e.Course.CourseName,
                    SemesterId = e.Course.SemesterId
                }
            }).ToList();
        }

        return business;
    }

    public static EnrollmentBusiness ToBusiness(EnrollmentEntity entity, bool includeStudent, bool includeCourse)
    {
        var business = new EnrollmentBusiness
        {
            EnrollmentId = entity.EnrollmentId,
            StudentId = entity.StudentId,
            CourseId = entity.CourseId,
            EnrollDate = entity.EnrollDate,
            Status = entity.Status
        };

        if (includeStudent && entity.Student != null)
        {
            business.Student = new StudentSummaryBusiness
            {
                StudentId = entity.Student.StudentId,
                FullName = entity.Student.FullName,
                Email = entity.Student.Email,
                DateOfBirth = entity.Student.DateOfBirth
            };
        }

        if (includeCourse && entity.Course != null)
        {
            business.Course = new CourseWithSemesterBusiness
            {
                CourseId = entity.Course.CourseId,
                CourseName = entity.Course.CourseName,
                SemesterId = entity.Course.SemesterId,
                Semester = entity.Course.Semester == null ? null : ToSemesterSummary(entity.Course.Semester)
            };
        }

        return business;
    }

    private static SemesterSummaryBusiness ToSemesterSummary(SemesterEntity semester) => new()
    {
        SemesterId = semester.SemesterId,
        SemesterName = semester.SemesterName,
        StartDate = semester.StartDate,
        EndDate = semester.EndDate
    };
}
