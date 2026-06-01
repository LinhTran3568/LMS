using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.BusinessModels;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Mappers;

namespace PRN232.LMS.Services.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _repository;
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;

    public EnrollmentService(
        IEnrollmentRepository repository,
        IStudentRepository studentRepository,
        ICourseRepository courseRepository)
    {
        _repository = repository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public async Task<EnrollmentBusiness?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id, includeStudent: true, includeCourse: true);
        return entity == null ? null : EntityMapper.ToBusiness(entity, includeStudent: true, includeCourse: true);
    }

    public async Task<PagedResultBusiness<EnrollmentBusiness>> GetAllAsync(ListQueryBusiness query)
    {
        var includeStudent = string.IsNullOrEmpty(query.Expand) || query.ShouldExpand("student");
        var includeCourse = string.IsNullOrEmpty(query.Expand) || query.ShouldExpand("course");
        var data = await _repository.GetPagedAsync(QueryMapper.ToDataQuery(query), includeStudent, includeCourse);
        return new PagedResultBusiness<EnrollmentBusiness>
        {
            Items = data.Items.Select(e => EntityMapper.ToBusiness(e, includeStudent, includeCourse)).ToList(),
            Page = data.Page,
            PageSize = data.PageSize,
            TotalItems = data.TotalItems
        };
    }

    public async Task<PagedResultBusiness<EnrollmentBusiness>> GetByCourseAsync(int courseId, ListQueryBusiness query)
    {
        var includeStudent = string.IsNullOrEmpty(query.Expand) || query.ShouldExpand("student");
        var includeCourse = string.IsNullOrEmpty(query.Expand) || query.ShouldExpand("course");
        var data = await _repository.GetPagedByCourseAsync(courseId, QueryMapper.ToDataQuery(query), includeStudent, includeCourse);
        return new PagedResultBusiness<EnrollmentBusiness>
        {
            Items = data.Items.Select(e => EntityMapper.ToBusiness(e, includeStudent, includeCourse)).ToList(),
            Page = data.Page,
            PageSize = data.PageSize,
            TotalItems = data.TotalItems
        };
    }

    public async Task<EnrollmentBusiness> CreateAsync(EnrollmentBusiness business)
    {
        if (!await _studentRepository.ExistsAsync(business.StudentId))
        {
            throw new InvalidOperationException("Student does not exist.");
        }

        if (!await _courseRepository.ExistsAsync(business.CourseId))
        {
            throw new InvalidOperationException("Course does not exist.");
        }

        var entity = new EnrollmentEntity
        {
            StudentId = business.StudentId,
            CourseId = business.CourseId,
            EnrollDate = business.EnrollDate,
            Status = business.Status
        };
        var created = await _repository.AddAsync(entity);
        return EntityMapper.ToBusiness(created, includeStudent: false, includeCourse: false);
    }

    public async Task<bool> UpdateAsync(int id, EnrollmentBusiness business)
    {
        if (!await _repository.ExistsAsync(id))
        {
            return false;
        }

        if (!await _studentRepository.ExistsAsync(business.StudentId))
        {
            throw new InvalidOperationException("Student does not exist.");
        }

        if (!await _courseRepository.ExistsAsync(business.CourseId))
        {
            throw new InvalidOperationException("Course does not exist.");
        }

        return await _repository.UpdateAsync(new EnrollmentEntity
        {
            EnrollmentId = id,
            StudentId = business.StudentId,
            CourseId = business.CourseId,
            EnrollDate = business.EnrollDate,
            Status = business.Status
        });
    }

    public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
}
