using ShiftManager.Domain.Models;

namespace ShiftManager.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetEmployeesAsync();
    Task<Employee?> GetEmployeeByIdAsync(int employeeId);
    Task<List<Employee>> GetEmployeesWithDepartmentAsync();
    Task<Employee?> GetEmployeeDetailsAsync(int employeeId); 
    Task<Employee> CreateNewEmployeeAsync(Employee employee);
    Task<Employee?> UpdateEmployeeAsync(int employeeId, string firstName, string lastName, string email, int departmentId);
    Task<bool> DeleteEmployeeAsync(int employeeId);
    Task<bool> IsValidEmployeeIdAsync(int employeeId);
    
}