using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.DTOs.Admin;

namespace ECommerce.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/permission-groups")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminPermissionGroupsController : ControllerBase
    {
        private readonly IPermissionGroupService _service;
        private readonly IPermissionGroupRepository _groupRepo;
        public AdminPermissionGroupsController(IPermissionGroupService service, IPermissionGroupRepository groupRepo)
        { _service = service; _groupRepo = groupRepo; }

        [HttpGet]
        public async Task<IActionResult> GetGroups([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null, [FromQuery] string? sortBy = null)
            => Ok(await _service.GetPagedAsync(page, pageSize, search, sortBy));

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreatePermissionGroupDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto.Name, dto.Description, dto.PermissionIds);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(Guid id, [FromBody] CreatePermissionGroupDto dto)
        {
            try
            {
                var result = await _service.UpdateAsync(id, dto.Name, dto.Description, dto.PermissionIds);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{groupId}/permissions")]
        public async Task<IActionResult> GetGroupPermissions(Guid groupId)
        {
            var permIds = await _groupRepo.GetPermissionIdsByGroupIdAsync(groupId); // need to inject IPermissionGroupRepository
            return Ok(permIds);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}