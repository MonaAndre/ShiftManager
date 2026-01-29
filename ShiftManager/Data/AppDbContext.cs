using Microsoft.EntityFrameworkCore;
using ShiftManager.Domain.Models;

namespace ShiftManager.Data;

public class AppDbContext : DbContext
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Role> Roles => Set<Role>();


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=shiftmanagerdb;Username=mona;Password=mona123");
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

        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                EmployeeId = 1,
                FirstName = "Anna",
                LastName = "Andersson",
                Email = "anna.andersson@company.se",
                DepartmentId = 2
            },
            new Employee
            {
                EmployeeId = 2,
                FirstName = "Erik",
                LastName = "Johansson",
                Email = "erik.johansson@company.se",
                DepartmentId = 3
            },
            new Employee
            {
                EmployeeId = 3,
                FirstName = "Sara",
                LastName = "Nilsson",
                Email = "sara.nilsson@company.se"
            }
        );


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
        
        modelBuilder.Entity<EmployeeRole>().HasData(
            new EmployeeRole
            {
                EmployeeId = 1,
                RoleId = 1 
            },
            new EmployeeRole
            {
                EmployeeId = 1,
                RoleId = 2 
            },
            new EmployeeRole
            {
                EmployeeId = 2,
                RoleId = 3 
            },
            new EmployeeRole
            {
                EmployeeId = 3,
                RoleId = 3 
            }
        );

    }
}