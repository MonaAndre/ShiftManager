using Microsoft.EntityFrameworkCore;
using ShiftManager.Domain.Interfaces;
using ShiftManager.Domain.Models;

namespace ShiftManager.Data.Repositories;

public class ShiftRepository : IShiftRepository

{
    private readonly AppDbContext _context;

    public ShiftRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Shift>> GetShiftsAsync()
    {
        return await _context.Shifts.AsNoTracking().Include(d => d.Department)
            .OrderBy(s => s.Department.DepartmentName)
            .ThenBy(s => s.StartDate)
            .ToListAsync();
    }

    public async Task<Shift?> FindShiftAsync(int id)
    {
        return await _context.Shifts.AsNoTracking()
            .Include(d => d.Department)
            .Include(e => e.Employee)
            .FirstOrDefaultAsync(s => s.ShiftId == id);
    }

    public async Task<Shift?> CreateShiftAsync(Shift shift)
    {
        try
        {
            await _context.Shifts.AddAsync(shift);
            await _context.SaveChangesAsync();
            return shift;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed creating new shift" + e.Message);
            Console.WriteLine(e.InnerException?.Message);
            return null;
        }
    }

    public async Task<Shift?> UpdateShiftAsync(int shiftId, DateTime start, DateTime end)
    {
        try
        {
            var shift = await _context.Shifts.FirstOrDefaultAsync(s => s.ShiftId == shiftId);
            if (shift is null) return null;

            if (end <= start) return null;

            var hasOverlap = await HasOverlappingShiftInternalAsync(
                shift.EmployeeId,
                shift.DepartmentId,
                start,
                end,
                excludeShiftId: shiftId);

            if (hasOverlap) return null;

            shift.StartDate = start;
            shift.EndDate = end;

            await _context.SaveChangesAsync();
            return shift;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }


    public async Task<bool> DeleteShiftAsync(int id)
    {
        var shift = await _context.Shifts.FirstOrDefaultAsync(s => s.ShiftId == id);
        if (shift is null) return false;
        _context.Shifts.Remove(shift);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsValidShiftIdAsync(int id)
    {
        return await _context.Shifts
            .AsNoTracking()
            .AnyAsync(s => s.ShiftId == id);
    }

    public async Task<bool> HasOverlappingShiftAsync(int employeeId, int departmentId, DateTime start, DateTime end)
    {
        return await _context.Shifts.AsNoTracking()
            .AnyAsync(s =>
                s.EmployeeId == employeeId &&
                s.DepartmentId == departmentId &&
                start < s.EndDate &&
                end > s.StartDate);
    }

    private async Task<bool> HasOverlappingShiftInternalAsync(
        int employeeId,
        int departmentId,
        DateTime start,
        DateTime end,
        int? excludeShiftId)
    {
        return await _context.Shifts.AsNoTracking()
            .AnyAsync(s =>
                s.EmployeeId == employeeId &&
                s.DepartmentId == departmentId &&
                (excludeShiftId == null || s.ShiftId != excludeShiftId.Value) &&
                start < s.EndDate &&
                end > s.StartDate
            );
    }

    public async Task<bool> PrintFutureShiftsAsync()
    {
        var nowUtc = DateTime.UtcNow;

        var shifts = await _context.Shifts
            .AsNoTracking()
            .Include(s => s.Department)
            .Include(s => s.Employee)
            .Where(s => s.StartDate > nowUtc)          // 👈 bara framtida
            .OrderBy(s => s.Department.DepartmentName)
            .ThenBy(s => s.StartDate)
            .ToListAsync();

        if (!shifts.Any())
        {
            Console.WriteLine("No future shifts available to edit.");
            return false;
        }

        Console.WriteLine();
        Console.WriteLine("ID | Department | Employee | Start | End");
        Console.WriteLine("------------------------------------------------------------");

        foreach (var s in shifts)
        {
            Console.WriteLine(
                $"{s.ShiftId} | " +
                $"{s.Department.DepartmentName} | " +
                $"{s.Employee.FirstName} {s.Employee.LastName} | " +
                $"{s.StartDate.ToLocalTime():yyyy-MM-dd HH:mm} | " +
                $"{s.EndDate.ToLocalTime():yyyy-MM-dd HH:mm}");
        }

        return true;
    }

}