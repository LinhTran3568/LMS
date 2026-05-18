using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Mappers;
using PRN232.LMS.API.Models.Common;
using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.API.Controllers;

[Route("api/enrollments")]
[Produces("application/json")]
[Consumes("application/json")]
public class EnrollmentsController : ApiControllerBase
{
    private readonly IEnrollmentService _service;

    public EnrollmentsController(IEnrollmentService service)
    {
        _service = service;
    }

    /// <summary>Get paginated enrollments.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] ListQueryRequest query)
    {
        var businessQuery = query.ToBusiness();
        var result = await _service.GetAllAsync(businessQuery);
        return Ok(PagedOk(result, e => ApiMapper.ToResponse(e), businessQuery.Fields));
    }

    /// <summary>Get enrollment by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, [FromQuery] string? fields)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound(ApiResponse<object>.Fail("Enrollment not found."));
        }

        return Ok(ApiResponse<object>.Ok(ApplyFields(ApiMapper.ToResponse(item), fields)));
    }

    /// <summary>Create an enrollment.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] EnrollmentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Status))
        {
            return BadRequest(ApiResponse<object>.Fail("Status is required."));
        }

        try
        {
            var created = await _service.CreateAsync(ApiMapper.ToBusiness(request));
            return CreatedAtAction(nameof(GetById), new { id = created.EnrollmentId },
                ApiResponse<object>.Ok(ApiMapper.ToResponse(created), "Enrollment created successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    /// <summary>Update an enrollment.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] EnrollmentRequest request)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, ApiMapper.ToBusiness(request));
            if (!updated)
            {
                return NotFound(ApiResponse<object>.Fail("Enrollment not found."));
            }

            return Ok(ApiResponse<object>.Ok(new { enrollmentId = id }, "Enrollment updated successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    /// <summary>Delete an enrollment.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(ApiResponse<object>.Fail("Enrollment not found."));
        }

        return NoContent();
    }
}
