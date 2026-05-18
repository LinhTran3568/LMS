using PRN232.LMS.Services.BusinessModels;
using PRN232.LMS.Services.Common;

namespace PRN232.LMS.Services.Interfaces;

public interface ISubjectService
{
    Task<SubjectBusiness?> GetByIdAsync(int id);
    Task<PagedResultBusiness<SubjectBusiness>> GetAllAsync(ListQueryBusiness query);
    Task<SubjectBusiness> CreateAsync(SubjectBusiness business);
    Task<bool> UpdateAsync(int id, SubjectBusiness business);
    Task<bool> DeleteAsync(int id);
}
