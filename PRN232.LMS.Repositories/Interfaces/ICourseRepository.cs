using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<CourseEntity?> GetByIdAsync(int id, bool includeSemester, bool includeEnrollments);
    Task<PagedDataResult<CourseEntity>> GetPagedAsync(DataQueryOptions options, bool includeSemester, bool includeEnrollments);
    Task<CourseEntity> AddAsync(CourseEntity entity);
    Task<bool> UpdateAsync(CourseEntity entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
