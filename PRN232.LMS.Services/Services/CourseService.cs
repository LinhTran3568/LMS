using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Services.BusinessModels;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Mappers;

namespace PRN232.LMS.Services.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ISemesterRepository _semesterRepository;

    public CourseService(
        ICourseRepository repository,
        IEnrollmentRepository enrollmentRepository,
        ISemesterRepository semesterRepository)
    {
        _repository = repository;
        _enrollmentRepository = enrollmentRepository;
        _semesterRepository = semesterRepository;
    }

    public async Task<CourseBusiness?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id, includeSemester: true, includeEnrollments: true);
        return entity == null ? null : EntityMapper.ToBusiness(entity, includeSemester: true, includeEnrollments: true);
    }

    public async Task<PagedResultBusiness<CourseBusiness>> GetAllAsync(ListQueryBusiness query)
    {
        var includeSemester = query.ShouldExpand("semester");
        var includeEnrollments = query.ShouldExpand("enrollments");
        var data = await _repository.GetPagedAsync(QueryMapper.ToDataQuery(query), includeSemester, includeEnrollments);
        return new PagedResultBusiness<CourseBusiness>
        {
            Items = data.Items.Select(e => EntityMapper.ToBusiness(e, includeSemester, includeEnrollments)).ToList(),
            Page = data.Page,
            PageSize = data.PageSize,
            TotalItems = data.TotalItems
        };
    }

    public async Task<PagedResultBusiness<EnrollmentBusiness>> GetEnrollmentsAsync(int courseId, ListQueryBusiness query)
    {
        var includeStudent = query.ShouldExpand("student");
        var data = await _enrollmentRepository.GetPagedByCourseAsync(
            courseId,
            QueryMapper.ToDataQuery(query),
            includeStudent);

        return new PagedResultBusiness<EnrollmentBusiness>
        {
            Items = data.Items.Select(e => EntityMapper.ToBusiness(e, includeStudent, includeCourse: false)).ToList(),
            Page = data.Page,
            PageSize = data.PageSize,
            TotalItems = data.TotalItems
        };
    }

    public async Task<CourseBusiness> CreateAsync(CourseBusiness business)
    {
        if (!await _semesterRepository.ExistsAsync(business.SemesterId))
        {
            throw new InvalidOperationException("Semester does not exist.");
        }

        var entity = new CourseEntity
        {
            CourseName = business.CourseName,
            SemesterId = business.SemesterId
        };
        var created = await _repository.AddAsync(entity);
        return EntityMapper.ToBusiness(created, includeSemester: false, includeEnrollments: false);
    }

    public async Task<bool> UpdateAsync(int id, CourseBusiness business)
    {
        if (!await _repository.ExistsAsync(id))
        {
            return false;
        }

        if (!await _semesterRepository.ExistsAsync(business.SemesterId))
        {
            throw new InvalidOperationException("Semester does not exist.");
        }

        return await _repository.UpdateAsync(new CourseEntity
        {
            CourseId = id,
            CourseName = business.CourseName,
            SemesterId = business.SemesterId
        });
    }

    public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
}
