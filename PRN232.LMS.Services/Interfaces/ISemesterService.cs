using PRN232.LMS.Services.BusinessModels;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.Services.Interfaces;

public interface ISemesterService
{
    Task<SemesterBusiness?> GetByIdAsync(int id);
    Task<PagedResultBusiness<SemesterBusiness>> GetAllAsync(ListQueryBusiness query);
    Task<SemesterBusiness> CreateAsync(SemesterBusiness business);
    Task<bool> UpdateAsync(int id, SemesterBusiness business);
    Task<bool> DeleteAsync(int id);
}
