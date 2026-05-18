using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;

namespace PRN232.LMS.Repositories.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly LmsDbContext _context;

    public StudentRepository(LmsDbContext context)
    {
        _context = context;
    }

    public async Task<StudentEntity?> GetByIdAsync(int id, bool includeEnrollments)
    {
        IQueryable<StudentEntity> query = _context.Students;
        if (includeEnrollments)
        {
            query = query
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .ThenInclude(c => c!.Semester);
        }

        return await query.FirstOrDefaultAsync(s => s.StudentId == id);
    }

    public async Task<PagedDataResult<StudentEntity>> GetPagedAsync(DataQueryOptions options, bool includeEnrollments)
    {
        IQueryable<StudentEntity> query = _context.Students.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(options.Search))
        {
            var keyword = options.Search.Trim();
            query = query.Where(s =>
                s.FullName.Contains(keyword) ||
                s.Email.Contains(keyword));
        }

        if (includeEnrollments)
        {
            query = query
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course);
        }

        var total = await query.CountAsync();
        query = query.ApplySort(options.Sort).ApplyPaging(options.Page, options.PageSize);
        var items = await query.ToListAsync();

        return new PagedDataResult<StudentEntity>
        {
            Items = items,
            TotalItems = total,
            Page = options.Page < 1 ? 1 : options.Page,
            PageSize = options.PageSize < 1 ? 10 : options.PageSize
        };
    }

    public async Task<StudentEntity> AddAsync(StudentEntity entity)
    {
        _context.Students.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(StudentEntity entity)
    {
        _context.Students.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Students.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _context.Students.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public Task<bool> ExistsAsync(int id) =>
        _context.Students.AnyAsync(s => s.StudentId == id);
}
