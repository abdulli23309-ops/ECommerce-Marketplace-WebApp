using ECommerce.Application.DTOs.Admin;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/admin/permissions")]
[Authorize(Roles = "SuperAdmin")]
public class AdminPermissionsController : ControllerBase
{
    private readonly IPermissionService _service;
    public AdminPermissionsController(IPermissionService service) => _service = service;

    [HttpGet("all")]
    public async Task<IActionResult> GetAllPermissions() => Ok(await _service.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto.Name, dto.Code, dto.Description);
            return Ok(result);
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePermission(Guid id, [FromBody] CreatePermissionDto dto)
    {
        try
        {
            var result = await _service.UpdateAsync(id, dto.Name, dto.Code, dto.Description);
            if (result == null) return NotFound();
            return Ok(result);
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePermission(Guid id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }
}