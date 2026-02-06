using ShiftManager.Domain.Models;

namespace ShiftManager.Domain.Interfaces.Repositories;

public interface IRoleRepository
{
    Task<List<Role>> GetRolesAsync();
    Task<Role> CreateRoleAsync(Role role);
    Task<Role?> UpdateRoleAsync(int id, string name, string description);
    Task<bool> DeleteRoleAsync(int id);
    Task<bool> IsValidRoleIdAsync(int roleId);
    Task PrintRolesAsync();
}