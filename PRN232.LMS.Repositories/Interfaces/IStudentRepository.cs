using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Interfaces;

public interface IStudentRepository
{
    Task<StudentEntity?> GetByIdAsync(int id, bool includeEnrollments);
    Task<PagedDataResult<StudentEntity>> GetPagedAsync(DataQueryOptions options, bool includeEnrollments);
    Task<StudentEntity> AddAsync(StudentEntity entity);
    Task<bool> UpdateAsync(StudentEntity entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
