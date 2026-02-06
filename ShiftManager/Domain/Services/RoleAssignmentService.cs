using Microsoft.EntityFrameworkCore;
using ShiftManager.Data;
using ShiftManager.Domain.Interfaces.Repositories;
using ShiftManager.Domain.Interfaces.Services;
using ShiftManager.Domain.Models;
using ShiftManager.DTOs;
using ShiftManager.Presentation;

namespace ShiftManager.Domain.Services;

public class RoleAssignmentService : IRoleAssignmentService
{
    private readonly AppDbContext _context;
    private readonly ConsoleHelpers _consoleHelpers;

    public RoleAssignmentService(AppDbContext context, ConsoleHelpers consoleHelpers)
    {
        _context = context;
        _consoleHelpers = consoleHelpers;
    }

    public async Task<bool> AssignRoleAsync(int employeeId, int roleId)
    {
        try
        {
            var exist = await _context.EmployeeRoles.AnyAsync(er => er.EmployeeId == employeeId && er.RoleId == roleId);
            if (exist)
            {
                Console.WriteLine("This employee already assigned to this role");
                return false;
            }

            var newAssignment = new EmployeeRole
            {
                EmployeeId = employeeId,
                RoleId = roleId,
            };
            _context.EmployeeRoles.Add(newAssignment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> RemoveRoleAsync(int employeeId, int roleId)
    {
        try
        {
            var assigmntToDelete =
                await _context.EmployeeRoles.FirstOrDefaultAsync(er =>
                    er.EmployeeId == employeeId && er.RoleId == roleId);
            if (assigmntToDelete == null)
            {
                Console.WriteLine("This employee is not assigned to this role");
                _consoleHelpers.Pause();
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failer to delete assigment {e.Message}");
            _consoleHelpers.Pause();
            return false;
        }
    }

    public async Task<EmployeeRolesDto?> GetRolesForEmployeeAsync(int employeeId)
    {
        var employee = await _context.Employees
            .AsNoTracking()
            .Where(e => e.EmployeeId == employeeId)
            .Select(e => new EmployeeRolesDto
            {
                EmployeeId = e.EmployeeId,
                EmployeeName = $"{e.FirstName} {e.LastName}",
                Roles = e.EmployeeRoles
                    .Select(er => new RoleDto
                    {
                        RoleId = er.RoleId,
                        RoleName = er.Role.RoleName,
                    })
                    .OrderBy(r => r.RoleId)
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (employee is null)
            return null;
        return employee;
    }

    public async Task<RoleEmployeesDto?> GetEmployeesForRoleAsync(int roleId)
    {
        var role = await _context.Roles
            .AsNoTracking()
            .Where(r => r.RoleId == roleId)
            .Select(r => new RoleEmployeesDto
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName,
                Employees = r.EmployeeRoles
                    .Select(er => er.Employee)
                    .OrderBy(e => e.LastName)
                    .ThenBy(e => e.FirstName)
                    .ToList()
            })
            .FirstOrDefaultAsync();
        if (role is null)
            return null;
        return role;
    }
}