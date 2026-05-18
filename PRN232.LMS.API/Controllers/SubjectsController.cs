using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Mappers;
using PRN232.LMS.API.Models.Common;
using PRN232.LMS.API.Models.Requests;
using PRN232.LMS.Services.Interfaces;

namespace PRN232.LMS.API.Controllers;

[Route("api/subjects")]
[Produces("application/json")]
[Consumes("application/json")]
public class SubjectsController : ApiControllerBase
{
    private readonly ISubjectService _service;

    public SubjectsController(ISubjectService service)
    {
        _service = service;
    }

    /// <summary>Get paginated subjects.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] ListQueryRequest query)
    {
        var businessQuery = query.ToBusiness();
        var result = await _service.GetAllAsync(businessQuery);
        return Ok(PagedOk(result, s => ApiMapper.ToResponse(s), businessQuery.Fields));
    }

    /// <summary>Get subject by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, [FromQuery] string? fields)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null)
        {
            return NotFound(ApiResponse<object>.Fail("Subject not found."));
        }

        return Ok(ApiResponse<object>.Ok(ApplyFields(ApiMapper.ToResponse(item), fields)));
    }

    /// <summary>Create a subject.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] SubjectRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.SubjectCode) || string.IsNullOrWhiteSpace(request.SubjectName))
        {
            return BadRequest(ApiResponse<object>.Fail("SubjectCode and SubjectName are required."));
        }

        var created = await _service.CreateAsync(ApiMapper.ToBusiness(request));
        return CreatedAtAction(nameof(GetById), new { id = created.SubjectId },
            ApiResponse<object>.Ok(ApiMapper.ToResponse(created), "Subject created successfully."));
    }

    /// <summary>Update a subject.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] SubjectRequest request)
    {
        var updated = await _service.UpdateAsync(id, ApiMapper.ToBusiness(request));
        if (!updated)
        {
            return NotFound(ApiResponse<object>.Fail("Subject not found."));
        }

        return Ok(ApiResponse<object>.Ok(new { subjectId = id }, "Subject updated successfully."));
    }

    /// <summary>Delete a subject.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(ApiResponse<object>.Fail("Subject not found."));
        }

        return NoContent();
    }
}
