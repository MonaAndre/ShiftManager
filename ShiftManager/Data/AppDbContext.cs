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
        
        modelBuilder.Entity<Role>(role =>
        {
            role.HasKey(r => r.RoleId);
            role.Property(r => r.RoleId).ValueGeneratedOnAdd();

            role.Property(r => r.RoleName).IsRequired().HasMaxLength(100);
            role.HasIndex(r => r.RoleName).IsUnique();

            role.Property(r => r.RoleDescription).IsRequired().HasMaxLength(400);
        });
    }
}