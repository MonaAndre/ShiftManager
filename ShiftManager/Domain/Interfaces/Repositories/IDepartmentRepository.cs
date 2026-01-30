using ShiftManager.Domain.Models;

namespace ShiftManager.Domain.Interfaces;

public interface IDepartmentRepository
{
    Task<List<Department>> GetDepartmentsAsync();
    Task<Department> CreateNewDepartmentAsync(string name);
    Task<Department?> UpdateDepartmentAsync(int id, string name);
    Task<bool> DeleteDepartmentAsync(int id);
    Task<bool> IsValidDepId(int departmentId);
}