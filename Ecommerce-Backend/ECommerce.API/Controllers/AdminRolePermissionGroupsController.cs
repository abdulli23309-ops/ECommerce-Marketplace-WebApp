using ECommerce.Application.Interfaces;
using ECommerce.Application.DTOs.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/admin/roles/{roleId}/permission-groups")]
[Authorize(Roles = "SuperAdmin")]
public class AdminRolePermissionGroupsController : ControllerBase
{
    private readonly IRolePermissionGroupService _service;
    public AdminRolePermissionGroupsController(IRolePermissionGroupService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetGroupsForRole(Guid roleId)
        => Ok(await _service.GetGroupIdsForRoleAsync(roleId));

    [HttpPost]
    public async Task<IActionResult> AssignGroup(Guid roleId, [FromBody] AssignPermissionGroupDto dto)
    {
        await _service.AssignGroupToRoleAsync(roleId, dto.PermissionGroupId);
        return NoContent();
    }

    [HttpDelete("{groupId}")]
    public async Task<IActionResult> RemoveGroup(Guid roleId, Guid groupId)
    {
        await _service.RemoveGroupFromRoleAsync(roleId, groupId);
        return NoContent();
    }
}