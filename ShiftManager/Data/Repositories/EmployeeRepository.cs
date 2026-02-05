using Microsoft.EntityFrameworkCore;
using ShiftManager.Domain.Interfaces;
using ShiftManager.Domain.Interfaces.Repositories;
using ShiftManager.Domain.Models;

namespace ShiftManager.Data.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
      private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Employee>> GetEmployeesAsync()
    {
        return await _context.Employees
            .AsNoTracking()
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync();
    }
    
    public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
    {
        return await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
    }
    
    public async Task<List<Employee>> GetEmployeesWithDepartmentAsync()
    {
        return await _context.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .OrderBy(e => e.EmployeeId)
            .ToListAsync();
    }
    
    public async Task<Employee?> GetEmployeeDetailsAsync(int employeeId)
    {
        return await _context.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .Include(e => e.EmployeeRoles)
                .ThenInclude(er => er.Role)
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
    }
    
    public async Task<Employee> CreateNewEmployeeAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }
    
    public async Task<Employee?> UpdateEmployeeAsync(
        int employeeId,
        string firstName,
        string lastName,
        string email,
        int departmentId)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

        if (employee is null)
            return null;

        employee.FirstName = firstName;
        employee.LastName = lastName;
        employee.Email = email;
        employee.DepartmentId = departmentId;

        await _context.SaveChangesAsync();
        return employee;
    }
    
    public async Task<bool> DeleteEmployeeAsync(int employeeId)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

        if (employee is null)
            return false;

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> IsValidEmployeeIdAsync(int employeeId)
    {
        return await _context.Employees
            .AsNoTracking()
            .AnyAsync(e => e.EmployeeId == employeeId);
    }

    public async Task<bool> IsValidEmployeeIdDepartmentAsync(int employeeId, int departmentId)
    {
        return await _context.Employees.AnyAsync(e => e.EmployeeId == employeeId && e.DepartmentId == departmentId);
        
    }
    public async Task<bool> PrintEmployeesByDepartmentAsync(int departmentId)
    {
        var employees = await _context.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .Where(e => e.DepartmentId == departmentId)
            .OrderBy(e => e.EmployeeId)
            .ToListAsync();

        if (!employees.Any())
        {
            Console.WriteLine("No employees found for this department.");
            return false;
        }

        var departmentName = employees.First().Department.DepartmentName;

        Console.WriteLine($"Employees from department: {departmentName}");
        Console.WriteLine("----------------------------------------");

        foreach (var e in employees)
        {
            Console.WriteLine(
                $"ID: {e.EmployeeId} | " +
                $"{e.FirstName} {e.LastName} | " +
                $"{e.Email}"
            );
        }

        Console.WriteLine("----------------------------------------");

        return true;
    }

    
}