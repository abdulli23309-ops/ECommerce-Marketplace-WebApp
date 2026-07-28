using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "SuperAdmin")]
public class AdminUsersController : ControllerBase
{
    private readonly IAdminUserService _service;
    public AdminUsersController(IAdminUserService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null, [FromQuery] string? role = null, [FromQuery] bool? isActive = null)
        => Ok(await _service.GetUsersAsync(page, pageSize, search, role, isActive));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _service.GetUserByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPut("{id}/activate")]
    public async Task<IActionResult> ActivateUser(Guid id)
    {
        await _service.ActivateUserAsync(id);
        return NoContent();
    }

    [HttpPut("{id}/deactivate")]
    public async Task<IActionResult> DeactivateUser(Guid id)
    {
        await _service.DeactivateUserAsync(id);
        return NoContent();
    }
}