using PRN232.LMS.Repositories.Common;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Interfaces;

public interface IEnrollmentRepository
{
    Task<EnrollmentEntity?> GetByIdAsync(int id, bool includeStudent, bool includeCourse);
    Task<PagedDataResult<EnrollmentEntity>> GetPagedAsync(DataQueryOptions options, bool includeStudent, bool includeCourse);
    Task<PagedDataResult<EnrollmentEntity>> GetPagedByCourseAsync(int courseId, DataQueryOptions options, bool includeStudent);
    Task<EnrollmentEntity> AddAsync(EnrollmentEntity entity);
    Task<bool> UpdateAsync(EnrollmentEntity entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
