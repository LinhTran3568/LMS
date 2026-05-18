using PRN232.LMS.Services.BusinessModels;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.Services.Interfaces;

public interface ICourseService
{
    Task<CourseBusiness?> GetByIdAsync(int id);
    Task<PagedResultBusiness<CourseBusiness>> GetAllAsync(ListQueryBusiness query);
    Task<CourseBusiness> CreateAsync(CourseBusiness business);
    Task<bool> UpdateAsync(int id, CourseBusiness business);
    Task<bool> DeleteAsync(int id);
}
