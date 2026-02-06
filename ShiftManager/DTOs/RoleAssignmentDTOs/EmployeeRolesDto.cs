namespace ShiftManager.DTOs;

public class EmployeeRolesDto
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = "";
    public IReadOnlyList<RoleDto> Roles { get; set; } = new List<RoleDto>();
}

public class RoleDto
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = "";
}