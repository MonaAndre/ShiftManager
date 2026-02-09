using Microsoft.EntityFrameworkCore;
using ShiftManager.Domain.Models;

namespace ShiftManager.Data;

public class AppDbContext : DbContext
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Shift> Shifts => Set<Shift>();
    public DbSet<EmployeeRole> EmployeeRoles => Set<EmployeeRole>();


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("your-connection-string");
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(department =>
        {
            department.HasKey(d => d.DepartmentId);
            department.Property(d => d.DepartmentId).ValueGeneratedOnAdd();
            department.Property(d => d.DepartmentName).IsRequired().HasMaxLength(100);
            department.HasIndex(d => d.DepartmentName).IsUnique();
        });

        modelBuilder.Entity<Department>().HasData(
            new Department { DepartmentId = 1, DepartmentName = "Unassigned" },
            new Department { DepartmentId = 2, DepartmentName = "IT" },
            new Department { DepartmentId = 3, DepartmentName = "Customer Support" }
        );

        modelBuilder.Entity<Employee>(employee =>
        {
            employee.HasKey(e => e.EmployeeId);
            employee.Property(e => e.EmployeeId).ValueGeneratedOnAdd();
            employee.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            employee.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            employee.Property(e => e.Email).IsRequired().HasMaxLength(200);
            employee.HasIndex(e => e.Email).IsUnique();
            employee.Property(e => e.DepartmentId).HasDefaultValue(1);
            employee.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Role>(role =>
        {
            role.HasKey(r => r.RoleId);
            role.Property(r => r.RoleId).ValueGeneratedOnAdd();

            role.Property(r => r.RoleName).IsRequired().HasMaxLength(100);
            role.HasIndex(r => r.RoleName).IsUnique();

            role.Property(r => r.RoleDescription).IsRequired().HasMaxLength(400);
        });

        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                RoleId = 1,
                RoleName = "Admin",
                RoleDescription = "System administrator"
            },
            new Role
            {
                RoleId = 2,
                RoleName = "Manager",
                RoleDescription = "Team or department manager"
            },
            new Role
            {
                RoleId = 3,
                RoleName = "Staff",
                RoleDescription = "Regular staff member"
            }
        );

        modelBuilder.Entity<EmployeeRole>(employeeRole =>
        {
            employeeRole.HasKey(er => new { er.EmployeeId, er.RoleId });

            employeeRole.HasOne(er => er.Employee)
                .WithMany(e => e.EmployeeRoles)
                .HasForeignKey(er => er.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            employeeRole.HasOne(er => er.Role)
                .WithMany(r => r.EmployeeRoles)
                .HasForeignKey(er => er.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Shift>(shift =>
        {
            shift.HasKey(s => s.ShiftId);
            shift.Property(s => s.ShiftId).ValueGeneratedOnAdd();
            shift.Property(s => s.EmployeeId);
            shift.HasOne(s => s.Employee)
                .WithMany(e => e.Shifts)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            shift.Property(s => s.DepartmentId);
            shift.HasOne(s => s.Department)
                .WithMany(d => d.Shifts)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
            shift.Property(s => s.StartDate).IsRequired();
            shift.Property(s => s.EndDate).IsRequired();
            shift.Property(s => s.CreatedAt)
                .HasDefaultValueSql("now()");
        });
    }
}