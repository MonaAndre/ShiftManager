using Microsoft.EntityFrameworkCore;
using ShiftManager.Data;
using ShiftManager.Domain.Interfaces;
using ShiftManager.Domain.Interfaces.Services;
using ShiftManager.Domain.Models;
using ShiftManager.Presentation;

namespace ShiftManager.Domain.Services;

public class SchedulingService : ISchedulingService
{
    private readonly AppDbContext _context;

    public SchedulingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, string Message, int CreatedCount)> PublishWeekForEmployeeAsync(
        int departmentId,
        int employeeId,
        DateTime weekStartDateUtc,
        TimeSpan dayStartTime,
        TimeSpan dayEndTime)
    {
        if (dayEndTime <= dayStartTime)
            return (false, "End time must be later than start time.", 0);

        if (weekStartDateUtc.Kind != DateTimeKind.Utc)
            return (false, "weekStartDateUtc must be UTC (DateTimeKind.Utc).", 0);

        var monday = weekStartDateUtc;

        var intervals = new List<(DateTime StartUtc, DateTime EndUtc)>();
        for (var i = 0; i < 5; i++)
        {
            var day = monday.AddDays(i);
            var startUtc = day.Add(dayStartTime);
            var endUtc = day.Add(dayEndTime);

            intervals.Add((startUtc, endUtc));
        }

        var weekStartUtc0 = monday;
        var weekEndUtc0 = monday.AddDays(5);
        var hasOverlap = await _context.Shifts
            .AsNoTracking()
            .AnyAsync(s =>
                s.EmployeeId == employeeId &&
                s.StartDate < weekEndUtc0 &&
                s.EndDate > weekStartUtc0);

        if (hasOverlap)
            return (false, "Cannot publish week schedule: employee has overlapping shift(s) in that week.", 0);
        await using var tx = await _context.Database.BeginTransactionAsync();

        try
        {
            var newShifts = intervals.Select(x => new Shift
            {
                EmployeeId = employeeId,
                DepartmentId = departmentId,
                StartDate = x.StartUtc,
                EndDate = x.EndUtc
            }).ToList();

            await _context.Shifts.AddRangeAsync(newShifts);
            var created = await _context.SaveChangesAsync();

            await tx.CommitAsync();
            return (true, $"Week schedule published. Created {created} shift(s).", created);
        }
        catch (DbUpdateException ex)
        {
            await tx.RollbackAsync();
            return (false,
                $"Failed to publish week schedule (DB update error): {ex.InnerException?.Message ?? ex.Message}", 0);
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return (false, $"Failed to publish week schedule: {ex.Message}", 0);
        }
    }
}