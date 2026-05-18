using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;

namespace PRN232.LMS.Repositories.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly LmsDbContext _context;

    public SubjectRepository(LmsDbContext context)
    {
        _context = context;
    }

    public Task<SubjectEntity?> GetByIdAsync(int id) =>
        _context.Subjects.AsNoTracking().FirstOrDefaultAsync(s => s.SubjectId == id);

    public async Task<PagedDataResult<SubjectEntity>> GetPagedAsync(DataQueryOptions options)
    {
        IQueryable<SubjectEntity> query = _context.Subjects.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(options.Search))
        {
            var keyword = options.Search.Trim();
            query = query.Where(s =>
                s.SubjectCode.Contains(keyword) ||
                s.SubjectName.Contains(keyword) ||
                s.Credit.ToString().Contains(keyword));
        }

        var total = await query.CountAsync();
        query = query.ApplySort(options.Sort).ApplyPaging(options.Page, options.PageSize);
        var items = await query.ToListAsync();

        return new PagedDataResult<SubjectEntity>
        {
            Items = items,
            TotalItems = total,
            Page = options.Page < 1 ? 1 : options.Page,
            PageSize = options.PageSize < 1 ? 10 : options.PageSize
        };
    }

    public async Task<SubjectEntity> AddAsync(SubjectEntity entity)
    {
        _context.Subjects.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(SubjectEntity entity)
    {
        _context.Subjects.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Subjects.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _context.Subjects.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public Task<bool> ExistsAsync(int id) =>
        _context.Subjects.AnyAsync(s => s.SubjectId == id);
}
