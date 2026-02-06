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
                .HasDefaultValueSql("now()"); //"timezone('utc',now()).ValueGeneratedOnAdd();"
        });
        var seedCreatedAt = new DateTime(2026, 1, 29, 12, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<Shift>().HasData(
            new Shift
            {
                ShiftId = 1,
                EmployeeId = 1,
                DepartmentId = 2,
                StartDate = new DateTime(2026, 2, 2, 8, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 2, 2, 16, 0, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            },
            new Shift
            {
                ShiftId = 2,
                EmployeeId = 1,
                DepartmentId = 2,
                StartDate = new DateTime(2026, 2, 3, 8, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 2, 3, 16, 0, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            },
            new Shift
            {
                ShiftId = 3,
                EmployeeId = 2,
                DepartmentId = 3,
                StartDate = new DateTime(2026, 2, 2, 9, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 2, 2, 17, 0, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            },
            new Shift
            {
                ShiftId = 4,
                EmployeeId = 2,
                DepartmentId = 3,
                StartDate = new DateTime(2026, 2, 3, 9, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 2, 3, 17, 0, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            },
            new Shift
            {
                ShiftId = 5,
                EmployeeId = 3,
                DepartmentId = 1,
                StartDate = new DateTime(2026, 2, 2, 10, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 2, 2, 18, 0, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            },
            new Shift
            {
                ShiftId = 6,
                EmployeeId = 3,
                DepartmentId = 1,
                StartDate = new DateTime(2026, 2, 3, 10, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 2, 3, 18, 0, 0, DateTimeKind.Utc),
                CreatedAt = seedCreatedAt
            }
        );
    }
}