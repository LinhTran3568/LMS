using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Repositories;

namespace PRN232.LMS.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositoryLayer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<LmsDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ISemesterRepository, SemesterRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

        return services;
    }
}
