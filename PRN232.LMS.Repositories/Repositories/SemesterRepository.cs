using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;

namespace PRN232.LMS.Repositories.Repositories;

public class SemesterRepository : ISemesterRepository
{
    private readonly LmsDbContext _context;

    public SemesterRepository(LmsDbContext context)
    {
        _context = context;
    }

    public async Task<SemesterEntity?> GetByIdAsync(int id, bool includeCourses)
    {
        IQueryable<SemesterEntity> query = _context.Semesters;
        if (includeCourses)
        {
            query = query.Include(s => s.Courses);
        }

        return await query.FirstOrDefaultAsync(s => s.SemesterId == id);
    }

    public async Task<PagedDataResult<SemesterEntity>> GetPagedAsync(DataQueryOptions options, bool includeCourses)
    {
        IQueryable<SemesterEntity> query = _context.Semesters.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(options.Search))
        {
            var keyword = options.Search.Trim();
            query = query.Where(s =>
                s.SemesterName.Contains(keyword) ||
                s.StartDate.ToString().Contains(keyword) ||
                s.EndDate.ToString().Contains(keyword));
        }

        if (includeCourses)
        {
            query = query.Include(s => s.Courses);
        }

        var total = await query.CountAsync();
        query = query.ApplySort(options.Sort).ApplyPaging(options.Page, options.PageSize);
        var items = await query.ToListAsync();

        return new PagedDataResult<SemesterEntity>
        {
            Items = items,
            TotalItems = total,
            Page = options.Page < 1 ? 1 : options.Page,
            PageSize = options.PageSize < 1 ? 10 : options.PageSize
        };
    }

    public async Task<SemesterEntity> AddAsync(SemesterEntity entity)
    {
        _context.Semesters.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(SemesterEntity entity)
    {
        _context.Semesters.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Semesters.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _context.Semesters.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public Task<bool> ExistsAsync(int id) =>
        _context.Semesters.AnyAsync(s => s.SemesterId == id);
}
