using PRN232.LMS.Services.BusinessModels;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.Services.Interfaces;

public interface IEnrollmentService
{
    Task<EnrollmentBusiness?> GetByIdAsync(int id);
    Task<PagedResultBusiness<EnrollmentBusiness>> GetAllAsync(ListQueryBusiness query);
    Task<PagedResultBusiness<EnrollmentBusiness>> GetByCourseAsync(int courseId, ListQueryBusiness query);
    Task<EnrollmentBusiness> CreateAsync(EnrollmentBusiness business);
    Task<bool> UpdateAsync(int id, EnrollmentBusiness business);
    Task<bool> DeleteAsync(int id);
}
