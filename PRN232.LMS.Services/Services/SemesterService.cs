using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.BusinessModels;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Mappers;

namespace PRN232.LMS.Services.Services;

public class SemesterService : ISemesterService
{
    private readonly ISemesterRepository _repository;

    public SemesterService(ISemesterRepository repository)
    {
        _repository = repository;
    }

    public async Task<SemesterBusiness?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id, includeCourses: true);
        return entity == null ? null : EntityMapper.ToBusiness(entity, includeCourses: true);
    }

    public async Task<PagedResultBusiness<SemesterBusiness>> GetAllAsync(ListQueryBusiness query)
    {
        var includeCourses = query.ShouldExpand("courses");
        var data = await _repository.GetPagedAsync(QueryMapper.ToDataQuery(query), includeCourses);
        return new PagedResultBusiness<SemesterBusiness>
        {
            Items = data.Items.Select(e => EntityMapper.ToBusiness(e, includeCourses)).ToList(),
            Page = data.Page,
            PageSize = data.PageSize,
            TotalItems = data.TotalItems
        };
    }

    public async Task<SemesterBusiness> CreateAsync(SemesterBusiness business)
    {
        var entity = new SemesterEntity
        {
            SemesterName = business.SemesterName,
            StartDate = business.StartDate,
            EndDate = business.EndDate
        };
        var created = await _repository.AddAsync(entity);
        return EntityMapper.ToBusiness(created, includeCourses: false);
    }

    public async Task<bool> UpdateAsync(int id, SemesterBusiness business)
    {
        if (!await _repository.ExistsAsync(id))
        {
            return false;
        }

        return await _repository.UpdateAsync(new SemesterEntity
        {
            SemesterId = id,
            SemesterName = business.SemesterName,
            StartDate = business.StartDate,
            EndDate = business.EndDate
        });
    }

    public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
}
