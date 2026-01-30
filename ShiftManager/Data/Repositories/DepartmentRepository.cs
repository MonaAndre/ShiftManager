using Microsoft.EntityFrameworkCore;
using ShiftManager.Domain.Interfaces;
using ShiftManager.Domain.Models;

namespace ShiftManager.Data.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;

    public DepartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Department>> GetDepartmentsAsync()
    {
        var result = await _context.Departments.AsNoTracking().OrderBy(d => d.DepartmentId).ToListAsync();
        return result;
    }

    public async Task<Department> CreateNewDepartmentAsync(string name)
    {
        var newDepartment = new Department
        {
            DepartmentName = name,
        };

        await _context.Departments.AddAsync(newDepartment);
        await _context.SaveChangesAsync();
        return newDepartment;
    }

    public async Task<Department?> UpdateDepartmentAsync(int id, string name)
    {
        var departmentToUpdate = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
        if (departmentToUpdate != null)
        {
            departmentToUpdate.DepartmentName = name;
            await _context.SaveChangesAsync();
            return departmentToUpdate;
        }

        return null;
    }

    public async Task<bool> DeleteDepartmentAsync(int id)
    {
        var departmentToDelete = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
        if (departmentToDelete != null)
        {
            _context.Departments.Remove(departmentToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> IsValidDepId(int departmentId)
    {
        return await _context.Departments
            .AsNoTracking()
            .AnyAsync(d => d.DepartmentId == departmentId);
    }
}