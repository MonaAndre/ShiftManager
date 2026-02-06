using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftManager.Domain.Models;

public class EmployeeRole
{
    public int EmployeeId { get; set; }
    public int RoleId { get; set; }

    [ForeignKey(nameof(EmployeeId))] public Employee Employee { get; set; }
    [ForeignKey(nameof(RoleId))] public Role Role { get; set; }
}