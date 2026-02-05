using ShiftManager.Domain.Models;

namespace ShiftManager.Domain.Interfaces;

public interface IShiftRepository
{
    Task<List<Shift>> GetShiftsAsync();
    Task<Shift?> FindShiftAsync(int id);
    Task<Shift?> CreateShiftAsync(Shift shift);
    Task<Shift?> UpdateShiftAsync(int shiftId, DateTime start, DateTime end);
    Task<bool> DeleteShiftAsync(int id);
    Task<bool> IsValidShiftIdAsync(int id);
    Task<bool> HasOverlappingShiftAsync(int employeeId, int departmentId, DateTime start, DateTime end);
    Task<bool> PrintFutureShiftsAsync();
}