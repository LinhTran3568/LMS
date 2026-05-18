using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Interfaces;

public interface ISubjectRepository
{
    Task<SubjectEntity?> GetByIdAsync(int id);
    Task<PagedDataResult<SubjectEntity>> GetPagedAsync(DataQueryOptions options);
    Task<SubjectEntity> AddAsync(SubjectEntity entity);
    Task<bool> UpdateAsync(SubjectEntity entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
