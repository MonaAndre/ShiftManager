using Microsoft.EntityFrameworkCore;
using ShiftManager.Domain.Interfaces;
using ShiftManager.Domain.Interfaces.Repositories;
using ShiftManager.Domain.Models;

namespace ShiftManager.Data.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Role>> GetRolesAsync()
    {
        return await _context.Roles.AsNoTracking().OrderBy(r => r.RoleId).ToListAsync();
    }

    public async Task<Role> CreateRoleAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task<Role?> UpdateRoleAsync(int id, string name, string description)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == id);
        if (role == null) return null;
        role.RoleName = name;
        role.RoleDescription = description;
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task<bool> DeleteRoleAsync(int id)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == id);
        if (role == null) return false;
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsValidRoleIdAsync(int roleId)
    {
        return await _context.Roles.AnyAsync(r => r.RoleId == roleId);
    }

    public async Task PrintRolesAsync()
    {
        try
        {
            var roles = await GetRolesAsync();
            if (roles.Count == 0)
            {
                Console.WriteLine("(no roles found)");
            }

            foreach (var r in roles)
                Console.WriteLine($"ID: {r.RoleId} | {r.RoleName} | {r.RoleDescription}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to load roles: " + ex.Message);
        }
    }
}