using ECommerce.Application.DTOs.Admin;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Interfaces.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/roles")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminRolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepo;
        public AdminRolesController(IRoleRepository roleRepo) => _roleRepo = roleRepo;

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleRepo.GetAllAsync();
            return Ok(roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name }));
        }
        [HttpGet("with-groups")]
        public async Task<IActionResult> GetRolesWithGroups()
        {
            var roles = await _roleRepo.GetRolesWithPermissionGroupsAsync();
            var result = roles.Select(r => new
            {
                r.Id,
                r.Name,
                PermissionGroupIds = r.RolePermissionGroups.Select(rpg => rpg.PermissionGroupId)
            });
            return Ok(result);
        }
    }
}