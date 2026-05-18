using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Mappers;
using PRN232.LMS.API.Models.Common;
using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.API.Controllers;

[Route("api/courses")]
[Produces("application/json")]
[Consumes("application/json")]
public class CoursesController : ApiControllerBase
{
    private readonly ICourseService _service;

    public CoursesController(ICourseService service)
    {
        _service = service;
    }

    /// <summary>Get paginated courses.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] ListQueryRequest query)
    {
        var businessQuery = query.ToBusiness();
        var result = await _service.GetAllAsync(businessQuery);
        return Ok(PagedOk(result, c => ApiMapper.ToResponse(c), businessQuery.Fields));
    }

    /// <summary>Get course by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, [FromQuery] string? fields)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound(ApiResponse<object>.Fail("Course not found."));
        }

        return Ok(ApiResponse<object>.Ok(ApplyFields(ApiMapper.ToResponse(item), fields)));
    }

    /// <summary>Create a course.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CourseRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CourseName))
        {
            return BadRequest(ApiResponse<object>.Fail("CourseName is required."));
        }

        try
        {
            var created = await _service.CreateAsync(ApiMapper.ToBusiness(request));
            return CreatedAtAction(nameof(GetById), new { id = created.CourseId },
                ApiResponse<object>.Ok(ApiMapper.ToResponse(created), "Course created successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    /// <summary>Update a course.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] CourseRequest request)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, ApiMapper.ToBusiness(request));
            if (!updated)
            {
                return NotFound(ApiResponse<object>.Fail("Course not found."));
            }

            return Ok(ApiResponse<object>.Ok(new { courseId = id }, "Course updated successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    /// <summary>Delete a course.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(ApiResponse<object>.Fail("Course not found."));
        }

        return NoContent();
    }
}
