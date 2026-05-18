using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;

namespace PRN232.LMS.Repositories.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly LmsDbContext _context;

    public EnrollmentRepository(LmsDbContext context)
    {
        _context = context;
    }

    public async Task<EnrollmentEntity?> GetByIdAsync(int id, bool includeStudent, bool includeCourse)
    {
        IQueryable<EnrollmentEntity> query = _context.Enrollments;
        if (includeStudent)
        {
            query = query.Include(e => e.Student);
        }

        if (includeCourse)
        {
            query = query
                .Include(e => e.Course)
                .ThenInclude(c => c!.Semester);
        }

        return await query.FirstOrDefaultAsync(e => e.EnrollmentId == id);
    }

    public async Task<PagedDataResult<EnrollmentEntity>> GetPagedAsync(
        DataQueryOptions options,
        bool includeStudent,
        bool includeCourse)
    {
        IQueryable<EnrollmentEntity> query = _context.Enrollments.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(options.Search))
        {
            var keyword = options.Search.Trim();
            query = query.Where(e =>
                e.Status.Contains(keyword) ||
                _context.Students.Any(s => s.StudentId == e.StudentId && s.FullName.Contains(keyword)) ||
                _context.Courses.Any(c => c.CourseId == e.CourseId && c.CourseName.Contains(keyword)));
        }

        if (includeStudent)
        {
            query = query.Include(e => e.Student);
        }

        if (includeCourse)
        {
            query = query.Include(e => e.Course);
        }

        var total = await query.CountAsync();
        query = query.ApplySort(options.Sort).ApplyPaging(options.Page, options.PageSize);
        var items = await query.ToListAsync();

        return new PagedDataResult<EnrollmentEntity>
        {
            Items = items,
            TotalItems = total,
            Page = options.Page < 1 ? 1 : options.Page,
            PageSize = options.PageSize < 1 ? 10 : options.PageSize
        };
    }

    public async Task<EnrollmentEntity> AddAsync(EnrollmentEntity entity)
    {
        _context.Enrollments.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(EnrollmentEntity entity)
    {
        _context.Enrollments.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Enrollments.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _context.Enrollments.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public Task<bool> ExistsAsync(int id) =>
        _context.Enrollments.AnyAsync(e => e.EnrollmentId == id);
}
