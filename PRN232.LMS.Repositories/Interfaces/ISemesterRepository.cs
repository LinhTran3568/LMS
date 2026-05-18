using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Interfaces;

public interface ISemesterRepository
{
    Task<SemesterEntity?> GetByIdAsync(int id, bool includeCourses);
    Task<PagedDataResult<SemesterEntity>> GetPagedAsync(DataQueryOptions options, bool includeCourses);
    Task<SemesterEntity> AddAsync(SemesterEntity entity);
    Task<bool> UpdateAsync(SemesterEntity entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
