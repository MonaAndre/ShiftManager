using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftManager.Domain.Models;

public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeeId { get; set; }

    [Required] [MaxLength(100)] public string FirstName { get; set; } = string.Empty;

    [Required] [MaxLength(100)] public string LastName { get; set; } = string.Empty;

    [Required] [MaxLength(200)] public string Email { get; set; } = string.Empty;
    [Required] public int DepartmentId { get; set; }

    [ForeignKey(nameof(DepartmentId))] public virtual Department Department { get; set; } = null!;
    
    public ICollection<EmployeeRole> EmployeeRoles { get; set; } = new List<EmployeeRole>();
}