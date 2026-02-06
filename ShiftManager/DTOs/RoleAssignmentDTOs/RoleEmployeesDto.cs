using ShiftManager.Domain.Models;

namespace ShiftManager.DTOs;

public class RoleEmployeesDto
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = "";
    public IReadOnlyList<Employee> Employees { get; set; } = new List<Employee>();
}