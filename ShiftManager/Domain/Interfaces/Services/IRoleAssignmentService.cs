using ShiftManager.Domain.Models;
using ShiftManager.DTOs;

namespace ShiftManager.Domain.Interfaces.Services;

public interface IRoleAssignmentService
{
    Task<bool> AssignRoleAsync(int employeeId, int roleId);
    Task<bool> RemoveRoleAsync(int employeeId, int roleId);

    Task<EmployeeRolesDto?> GetRolesForEmployeeAsync(int employeeId);
    Task<RoleEmployeesDto?> GetEmployeesForRoleAsync(int roleId);
}