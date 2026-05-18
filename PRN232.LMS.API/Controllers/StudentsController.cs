using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Mappers;
using PRN232.LMS.API.Models.Common;
using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.API.Controllers;

[Route("api/students")]
[Produces("application/json")]
[Consumes("application/json")]
public class StudentsController : ApiControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    {
        _service = service;
    }

    /// <summary>Get paginated students.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] ListQueryRequest query)
    {
        var businessQuery = query.ToBusiness();
        var result = await _service.GetAllAsync(businessQuery);
        return Ok(PagedOk(result, s => ApiMapper.ToResponse(s), businessQuery.Fields));
    }

    /// <summary>Get student by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, [FromQuery] string? fields)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound(ApiResponse<object>.Fail("Student not found."));
        }

        return Ok(ApiResponse<object>.Ok(ApplyFields(ApiMapper.ToResponse(item), fields)));
    }

    /// <summary>Create a student.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] StudentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) || string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(ApiResponse<object>.Fail("FullName and Email are required."));
        }

        var created = await _service.CreateAsync(ApiMapper.ToBusiness(request));
        return CreatedAtAction(nameof(GetById), new { id = created.StudentId },
            ApiResponse<object>.Ok(ApiMapper.ToResponse(created), "Student created successfully."));
    }

    /// <summary>Update a student.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] StudentRequest request)
    {
        var updated = await _service.UpdateAsync(id, ApiMapper.ToBusiness(request));
        if (!updated)
        {
            return NotFound(ApiResponse<object>.Fail("Student not found."));
        }

        return Ok(ApiResponse<object>.Ok(new { studentId = id }, "Student updated successfully."));
    }

    /// <summary>Delete a student.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(ApiResponse<object>.Fail("Student not found."));
        }

        return NoContent();
    }
}
