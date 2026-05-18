using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.BusinessModels;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Mappers;

namespace PRN232.LMS.Services.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<StudentBusiness?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id, includeEnrollments: true);
        return entity == null ? null : EntityMapper.ToBusiness(entity, includeEnrollments: true);
    }

    public async Task<PagedResultBusiness<StudentBusiness>> GetAllAsync(ListQueryBusiness query)
    {
        var includeEnrollments = query.ShouldExpand("enrollments");
        var data = await _repository.GetPagedAsync(QueryMapper.ToDataQuery(query), includeEnrollments);
        return new PagedResultBusiness<StudentBusiness>
        {
            Items = data.Items.Select(e => EntityMapper.ToBusiness(e, includeEnrollments)).ToList(),
            Page = data.Page,
            PageSize = data.PageSize,
            TotalItems = data.TotalItems
        };
    }

    public async Task<StudentBusiness> CreateAsync(StudentBusiness business)
    {
        var entity = new StudentEntity
        {
            FullName = business.FullName,
            Email = business.Email,
            DateOfBirth = business.DateOfBirth
        };
        var created = await _repository.AddAsync(entity);
        return EntityMapper.ToBusiness(created, includeEnrollments: false);
    }

    public async Task<bool> UpdateAsync(int id, StudentBusiness business)
    {
        if (!await _repository.ExistsAsync(id))
        {
            return false;
        }

        return await _repository.UpdateAsync(new StudentEntity
        {
            StudentId = id,
            FullName = business.FullName,
            Email = business.Email,
            DateOfBirth = business.DateOfBirth
        });
    }

    public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
}
