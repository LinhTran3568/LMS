using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Data;

public class LmsDbContext : DbContext
{
    public LmsDbContext(DbContextOptions<LmsDbContext> options) : base(options)
    {
    }

    public DbSet<SemesterEntity> Semesters => Set<SemesterEntity>();
    public DbSet<CourseEntity> Courses => Set<CourseEntity>();
    public DbSet<SubjectEntity> Subjects => Set<SubjectEntity>();
    public DbSet<StudentEntity> Students => Set<StudentEntity>();
    public DbSet<EnrollmentEntity> Enrollments => Set<EnrollmentEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SemesterEntity>(entity =>
        {
            entity.ToTable("Semester");
            entity.HasKey(e => e.SemesterId);
            entity.Property(e => e.SemesterName).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<CourseEntity>(entity =>
        {
            entity.ToTable("Course");
            entity.HasKey(e => e.CourseId);
            entity.Property(e => e.CourseName).HasMaxLength(100).IsRequired();
            entity.HasOne(e => e.Semester)
                .WithMany(s => s.Courses)
                .HasForeignKey(e => e.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SubjectEntity>(entity =>
        {
            entity.ToTable("Subject");
            entity.HasKey(e => e.SubjectId);
            entity.Property(e => e.SubjectCode).HasMaxLength(20).IsRequired();
            entity.Property(e => e.SubjectName).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<StudentEntity>(entity =>
        {
            entity.ToTable("Student");
            entity.HasKey(e => e.StudentId);
            entity.Property(e => e.FullName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<EnrollmentEntity>(entity =>
        {
            entity.ToTable("Enrollment");
            entity.HasKey(e => e.EnrollmentId);
            entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
            entity.HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
