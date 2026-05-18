using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Seed;

public static class DataSeeder
{
    private static readonly string[] Statuses = ["Active", "Completed", "Dropped", "Pending"];

    public static void Seed(LmsDbContext context)
    {
        if (context.Semesters.Any())
        {
            return;
        }

        var semesters = new List<SemesterEntity>();
        for (var i = 1; i <= 5; i++)
        {
            var start = new DateTime(2022 + i, 1, 15);
            semesters.Add(new SemesterEntity
            {
                SemesterName = $"Semester {i} - {(start.Year)}/{(start.Year + 1)}",
                StartDate = start,
                EndDate = start.AddMonths(5)
            });
        }
        context.Semesters.AddRange(semesters);
        context.SaveChanges();

        var subjects = new List<SubjectEntity>();
        for (var i = 1; i <= 10; i++)
        {
            subjects.Add(new SubjectEntity
            {
                SubjectCode = $"SUB{i:D3}",
                SubjectName = $"Subject {i}",
                Credit = 2 + (i % 4)
            });
        }
        context.Subjects.AddRange(subjects);
        context.SaveChanges();

        var courses = new List<CourseEntity>();
        var random = new Random(42);
        for (var i = 1; i <= 20; i++)
        {
            courses.Add(new CourseEntity
            {
                CourseName = $"Course {i}",
                SemesterId = semesters[random.Next(semesters.Count)].SemesterId
            });
        }
        context.Courses.AddRange(courses);
        context.SaveChanges();

        var students = new List<StudentEntity>();
        for (var i = 1; i <= 50; i++)
        {
            students.Add(new StudentEntity
            {
                FullName = $"Student {i} Nguyen",
                Email = $"student{i}@lms.edu.vn",
                DateOfBirth = new DateTime(2000, 1, 1).AddDays(i * 30)
            });
        }
        context.Students.AddRange(students);
        context.SaveChanges();

        var enrollments = new List<EnrollmentEntity>();
        for (var i = 0; i < 500; i++)
        {
            enrollments.Add(new EnrollmentEntity
            {
                StudentId = students[random.Next(students.Count)].StudentId,
                CourseId = courses[random.Next(courses.Count)].CourseId,
                EnrollDate = DateTime.UtcNow.AddDays(-random.Next(1, 365)),
                Status = Statuses[random.Next(Statuses.Length)]
            });
        }
        context.Enrollments.AddRange(enrollments);
        context.SaveChanges();
    }
}
