namespace ShiftManager.Domain.Interfaces.Services;

public interface IReportsService
{
    Task<bool> PrintDepartmentSummaryAsync(int departmentId);
}