using Microsoft.EntityFrameworkCore;
using ShiftManager.Data;
using ShiftManager.Domain.Interfaces.Services;
using ShiftManager.Presentation;

namespace ShiftManager.Domain.Services;

public class ReportsService :IReportsService
{
    private readonly AppDbContext _context;
    private readonly ConsoleHelpers _consoleHelpers;

    public ReportsService(AppDbContext context, ConsoleHelpers consoleHelpers)
    {
        _context = context;
        _consoleHelpers = consoleHelpers;
    }
    public async Task<bool> PrintDepartmentSummaryAsync(int departmentId)
    {
        try
        {
            var dept = await _context.Departments
                .AsNoTracking()
                .Where(d => d.DepartmentId == departmentId)
                .Select(d => new
                {
                    d.DepartmentName,
                    EmployeeCount = d.Employees.Count(),
                    ShiftCount = d.Shifts.Count()
                })
                .SingleOrDefaultAsync();

            if (dept is null)
            {
                Console.WriteLine("Department not found.");
                return false;
            }

            Console.WriteLine($"Department: {dept.DepartmentName}");
            Console.WriteLine($"Employees:  {dept.EmployeeCount}");
            Console.WriteLine($"Shifts:     {dept.ShiftCount}");
           _consoleHelpers.Pause();
           return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine("Failed to get department summary.");
            _consoleHelpers.Pause();
            return false;
        }
    }

}