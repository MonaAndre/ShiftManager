namespace ShiftManager.Domain.Interfaces.Services;

public interface ISchedulingService
{
    Task<(bool Success, string Message, int CreatedCount)> PublishWeekForEmployeeAsync(int departmentId,
        int employeeId,
        DateTime weekStartDateUtc,
        TimeSpan dayStartTime,
        TimeSpan dayEndTime);
}