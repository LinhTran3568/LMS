using PRN232.LMS.Services.BusinessModels;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.Services.Interfaces;

public interface IStudentService
{
    Task<StudentBusiness?> GetByIdAsync(int id);
    Task<PagedResultBusiness<StudentBusiness>> GetAllAsync(ListQueryBusiness query);
    Task<StudentBusiness> CreateAsync(StudentBusiness business);
    Task<bool> UpdateAsync(int id, StudentBusiness business);
    Task<bool> DeleteAsync(int id);
}
