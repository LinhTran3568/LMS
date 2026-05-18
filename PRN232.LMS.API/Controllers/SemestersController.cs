using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Mappers;
using PRN232.LMS.API.Models.Common;
using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.API.Controllers;

[Route("api/semesters")]
[Produces("application/json")]
[Consumes("application/json")]
public class SemestersController : ApiControllerBase
{
    private readonly ISemesterService _service;

    public SemestersController(ISemesterService service)
    {
        _service = service;
    }

    /// <summary>Get paginated semesters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] ListQueryRequest query)
    {
        var businessQuery = query.ToBusiness();
        var result = await _service.GetAllAsync(businessQuery);
        return Ok(PagedOk(result, s => ApiMapper.ToResponse(s), businessQuery.Fields));
    }

    /// <summary>Get semester by id (includes related courses).</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, [FromQuery] string? fields)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound(ApiResponse<object>.Fail("Semester not found."));
        }

        var response = ApplyFields(ApiMapper.ToResponse(item), fields);
        return Ok(ApiResponse<object>.Ok(response));
    }

    /// <summary>Create a semester.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] SemesterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.SemesterName))
        {
            return BadRequest(ApiResponse<object>.Fail("SemesterName is required."));
        }

        var created = await _service.CreateAsync(ApiMapper.ToBusiness(request));
        var response = ApiMapper.ToResponse(created);
        return CreatedAtAction(nameof(GetById), new { id = created.SemesterId },
            ApiResponse<object>.Ok(response, "Semester created successfully."));
    }

    /// <summary>Update a semester.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] SemesterRequest request)
    {
        var updated = await _service.UpdateAsync(id, ApiMapper.ToBusiness(request));
        if (!updated)
        {
            return NotFound(ApiResponse<object>.Fail("Semester not found."));
        }

        return Ok(ApiResponse<object>.Ok(new { semesterId = id }, "Semester updated successfully."));
    }

    /// <summary>Delete a semester.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(ApiResponse<object>.Fail("Semester not found."));
        }

        return NoContent();
    }
}
