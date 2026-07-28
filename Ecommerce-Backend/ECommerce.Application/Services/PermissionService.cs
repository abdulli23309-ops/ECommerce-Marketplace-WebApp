using ECommerce.Application.DTOs.Admin;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.Admin
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repo;

        public PermissionService(IPermissionRepository repo) => _repo = repo;

        public async Task<IEnumerable<PermissionDto>> GetAllAsync()
        {
            var perms = await _repo.GetAllAsync();
            return perms.Select(Map);
        }

        public async Task<PermissionDto> CreateAsync(string name, string code, string? description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.");
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code is required.");

            if (await _repo.ExistsByCodeAsync(code))
                throw new InvalidOperationException("A permission with this code already exists.");

            var perm = new Permission
            {
                Name = name.Trim(),
                Code = code.Trim().ToLower(),
                Description = description?.Trim()
            };
            await _repo.AddAsync(perm);
            await _repo.SaveChangesAsync();
            return Map(perm);
        }

        public async Task<PermissionDto?> UpdateAsync(Guid id, string name, string code, string? description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.");
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code is required.");

            var perm = await _repo.GetByIdAsync(id);
            if (perm == null) return null;

            if (await _repo.ExistsByCodeAsync(code, excludeId: id))
                throw new InvalidOperationException("A permission with this code already exists.");

            perm.Name = name.Trim();
            perm.Code = code.Trim().ToLower();
            perm.Description = description?.Trim();
            _repo.Update(perm);
            await _repo.SaveChangesAsync();
            return Map(perm);
        }

        public async Task DeleteAsync(Guid id)
        {
            var perm = await _repo.GetByIdAsync(id)
                       ?? throw new InvalidOperationException("Permission not found.");
            _repo.Delete(perm);
            await _repo.SaveChangesAsync();
        }

        private static PermissionDto Map(Permission p) => new PermissionDto
        {
            Id = p.Id,
            Name = p.Name,
            Code = p.Code,
            Description = p.Description
        };
    }
}