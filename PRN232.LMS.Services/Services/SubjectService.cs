using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.BusinessModels;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Mappers;

namespace PRN232.LMS.Services.Services;

public class SubjectService : ISubjectService
{
    private readonly ISubjectRepository _repository;

    public SubjectService(ISubjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<SubjectBusiness?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity == null ? null : EntityMapper.ToBusiness(entity);
    }

    public async Task<PagedResultBusiness<SubjectBusiness>> GetAllAsync(ListQueryBusiness query)
    {
        var data = await _repository.GetPagedAsync(QueryMapper.ToDataQuery(query));
        return new PagedResultBusiness<SubjectBusiness>
        {
            Items = data.Items.Select(EntityMapper.ToBusiness).ToList(),
            Page = data.Page,
            PageSize = data.PageSize,
            TotalItems = data.TotalItems
        };
    }

    public async Task<SubjectBusiness> CreateAsync(SubjectBusiness business)
    {
        var entity = new SubjectEntity
        {
            SubjectCode = business.SubjectCode,
            SubjectName = business.SubjectName,
            Credit = business.Credit
        };
        var created = await _repository.AddAsync(entity);
        return EntityMapper.ToBusiness(created);
    }

    public async Task<bool> UpdateAsync(int id, SubjectBusiness business)
    {
        if (!await _repository.ExistsAsync(id))
        {
            return false;
        }

        return await _repository.UpdateAsync(new SubjectEntity
        {
            SubjectId = id,
            SubjectCode = business.SubjectCode,
            SubjectName = business.SubjectName,
            Credit = business.Credit
        });
    }

    public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
}
