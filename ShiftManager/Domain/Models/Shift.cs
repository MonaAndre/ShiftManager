using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftManager.Domain.Models;

public class Shift
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ShiftId { get; set; }

    public int EmployeeId { get; set; }
    [ForeignKey(nameof(EmployeeId))] public Employee Employee { get; set; } = null!;
    public int DepartmentId { get; set; }
    [ForeignKey(nameof(DepartmentId))] public Department Department { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
}