using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;

namespace PRN232.LMS.Repositories.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly LmsDbContext _context;

    public CourseRepository(LmsDbContext context)
    {
        _context = context;
    }

    public async Task<CourseEntity?> GetByIdAsync(int id, bool includeSemester, bool includeEnrollments)
    {
        IQueryable<CourseEntity> query = _context.Courses;
        if (includeSemester)
        {
            query = query.Include(c => c.Semester);
        }

        if (includeEnrollments)
        {
            query = query.Include(c => c.Enrollments);
        }

        return await query.FirstOrDefaultAsync(c => c.CourseId == id);
    }

    public async Task<PagedDataResult<CourseEntity>> GetPagedAsync(
        DataQueryOptions options,
        bool includeSemester,
        bool includeEnrollments)
    {
        IQueryable<CourseEntity> query = _context.Courses.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(options.Search))
        {
            var keyword = options.Search.Trim();
            query = query.Where(c =>
                c.CourseName.Contains(keyword) ||
                c.SemesterId.ToString().Contains(keyword));
        }

        if (includeSemester)
        {
            query = query.Include(c => c.Semester);
        }

        if (includeEnrollments)
        {
            query = query.Include(c => c.Enrollments);
        }

        var total = await query.CountAsync();
        query = query.ApplySort(options.Sort).ApplyPaging(options.Page, options.PageSize);
        var items = await query.ToListAsync();

        return new PagedDataResult<CourseEntity>
        {
            Items = items,
            TotalItems = total,
            Page = options.Page < 1 ? 1 : options.Page,
            PageSize = options.PageSize < 1 ? 10 : options.PageSize
        };
    }

    public async Task<CourseEntity> AddAsync(CourseEntity entity)
    {
        _context.Courses.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(CourseEntity entity)
    {
        _context.Courses.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Courses.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _context.Courses.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public Task<bool> ExistsAsync(int id) =>
        _context.Courses.AnyAsync(c => c.CourseId == id);
}
