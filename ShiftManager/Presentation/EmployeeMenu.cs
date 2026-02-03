using System.Text.RegularExpressions;
using ShiftManager.Domain.Interfaces;
using ShiftManager.Domain.Models;

namespace ShiftManager.Presentation;

public class EmployeeMenu
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ConsoleHelpers _consoleHelpers;

    public EmployeeMenu(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository,  ConsoleHelpers consoleHelpers)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _consoleHelpers = consoleHelpers;
    }

    private void OpenEmployeeMenu()
    {
        Console.Clear();
        Console.WriteLine("----------- Employees management ------------");
        Console.WriteLine("You can:");
        Console.WriteLine("1. List employees (with department)");
        Console.WriteLine("2. View employee details (department + roles)");
        Console.WriteLine("3. Add new employee");
        Console.WriteLine("4. Update employee");
        Console.WriteLine("5. Delete employee");
        Console.WriteLine("6. Back to main menu");
        Console.WriteLine("--------------------------------------------");
    }

    public async Task RunEmployeeAsync()
    {
        while (true)
        {
            OpenEmployeeMenu();
            Console.Write("Choose option 1-6: ");
            var input = Console.ReadLine()?.Trim();

            if (!int.TryParse(input, out var choice))
            {
                Console.WriteLine("Invalid choice. Please enter a number.");
                _consoleHelpers.Pause();
                continue;
            }

            switch (choice)
            {
                case 1:
                    await ListEmployeesAsync();
                    break;
                case 2:
                    await ViewEmployeeDetailsAsync();
                    break;
                case 3:
                    await AddEmployeeAsync();
                    break;
                case 4:
                    await EditEmployeeAsync();
                    break;
                case 5:
                    await DeleteEmployeeAsync();
                    break;
                case 6:
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                   _consoleHelpers.Pause();
                    break;
            }
        }
    }

    private async Task ListEmployeesAsync()
    {
        Console.Clear();
        Console.WriteLine("Employees (with department)\n");

        try
        {
            var employees = await _employeeRepository.GetEmployeesWithDepartmentAsync();

            if (employees.Count == 0)
            {
                Console.WriteLine("No employees found.");
                _consoleHelpers.Pause();
                return;
            }

            foreach (var e in employees)
            {
                var depName = e.Department?.DepartmentName ?? "(no department)";
                Console.WriteLine($"ID: {e.EmployeeId} | {e.FirstName} {e.LastName} | {e.Email} | Dept: {depName}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to list employees: " + ex.Message);
        }

        _consoleHelpers.Pause();
    }

    private async Task ViewEmployeeDetailsAsync()
    {
        Console.Clear();
        Console.WriteLine("View employee details\n");

        var id = _consoleHelpers.ReadInt("Enter employee id: ");
        if (id is null) return;

        try
        {
            var exists = await _employeeRepository.IsValidEmployeeIdAsync(id.Value);
            if (!exists)
            {
                Console.WriteLine("Could not find employee with this id.");
                _consoleHelpers.Pause();
                return;
            }

            var employee = await _employeeRepository.GetEmployeeDetailsAsync(id.Value);
            if (employee is null)
            {
                Console.WriteLine("Could not load employee details.");
                _consoleHelpers.Pause();
                return;
            }

            Console.WriteLine($"ID: {employee.EmployeeId}");
            Console.WriteLine($"Name: {employee.FirstName} {employee.LastName}");
            Console.WriteLine($"Email: {employee.Email}");
            Console.WriteLine($"Department: {employee.Department?.DepartmentName ?? "(no department)"}");

            Console.WriteLine("\nRoles:");
            if (employee.EmployeeRoles.Count == 0)
            {
                Console.WriteLine("- (no roles)");
            }
            else
            {
                foreach (var er in employee.EmployeeRoles)
                {
                    var roleName = er.Role?.RoleName ?? "(unknown role)";
                    Console.WriteLine($"- {roleName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to get employee details: " + ex.Message);
        }

        _consoleHelpers.Pause();
    }

    private async Task AddEmployeeAsync()
    {
        Console.Clear();
        Console.WriteLine("Add new employee\n");

        var firstName = _consoleHelpers.ReadNonEmptyString("First name: ");
        if (firstName is null) return;

        var lastName = _consoleHelpers.ReadNonEmptyString("Last name: ");
        if (lastName is null) return;

        var email = _consoleHelpers.ReadNonEmptyString("Email: ");
        if (email is null) return;
        var isValid = Regex.IsMatch(
            email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!isValid)
        {
            Console.WriteLine("Invalid email format.");
            _consoleHelpers.Pause();
            return;
        }

        Console.WriteLine("\nAvailable departments:");
        await PrintDepartmentsAsync();

        var depId = _consoleHelpers.ReadInt("Department id: ");
        if (depId is null) return;

        try
        {
            var isValidDep = await _departmentRepository.IsValidDepIdAsync(depId.Value);
            if (!isValidDep)
            {
                Console.WriteLine("Invalid department id.");
                _consoleHelpers.Pause();
                return;
            }

            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                DepartmentId = depId.Value
            };

            var created = await _employeeRepository.CreateNewEmployeeAsync(employee);
            Console.WriteLine($"\nEmployee created: ID {created.EmployeeId} | {created.FirstName} {created.LastName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to create employee: " + ex.Message);
        }

        _consoleHelpers.Pause();
    }

    private async Task EditEmployeeAsync()
    {
        Console.Clear();
        Console.WriteLine("Update employee\n");

        var id = _consoleHelpers.ReadInt("Enter employee id: ");
        if (id is null) return;

        try
        {
            var exists = await _employeeRepository.IsValidEmployeeIdAsync(id.Value);
            if (!exists)
            {
                Console.WriteLine("Could not find employee with this id.");
                _consoleHelpers.Pause();
                return;
            }

            var firstName = _consoleHelpers.ReadNonEmptyString("New first name: ");
            if (firstName is null) return;

            var lastName = _consoleHelpers.ReadNonEmptyString("New last name: ");
            if (lastName is null) return;

            var email = _consoleHelpers.ReadNonEmptyString("New email: ");
            if (email is null) return;
            var isValid = Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!isValid)
            {
                Console.WriteLine("Invalid email format.");
                _consoleHelpers.Pause();
                return;
            }

            Console.WriteLine("\nAvailable departments:");
            await PrintDepartmentsAsync();

            var depId = _consoleHelpers.ReadInt("New department id: ");
            if (depId is null) return;

            var isValidDep = await _departmentRepository.IsValidDepIdAsync(depId.Value);
            if (!isValidDep)
            {
                Console.WriteLine("Invalid department id.");
                _consoleHelpers.Pause();
                return;
            }

            var updated = await _employeeRepository.UpdateEmployeeAsync(
                id.Value,
                firstName,
                lastName,
                email,
                depId.Value
            );

            if (updated is null)
            {
                Console.WriteLine("Could not update employee.");
                _consoleHelpers.Pause();
                return;
            }

            Console.WriteLine($"\nEmployee updated: ID {updated.EmployeeId} | {updated.FirstName} {updated.LastName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to update employee: " + ex.Message);
        }

        _consoleHelpers.Pause();
    }

    private async Task DeleteEmployeeAsync()
    {
        Console.Clear();
        Console.WriteLine("Delete employee\n");

        var id = _consoleHelpers.ReadInt("Enter employee id: ");
        if (id is null) return;

        try
        {
            var exists = await _employeeRepository.IsValidEmployeeIdAsync(id.Value);
            if (!exists)
            {
                Console.WriteLine("Could not find employee with this id.");
                _consoleHelpers.Pause();
                return;
            }

            var deleted = await _employeeRepository.DeleteEmployeeAsync(id.Value);
            Console.WriteLine(deleted
                ? $"Employee deleted: ID {id.Value}"
                : "Employee was not deleted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to delete employee: " + ex.Message);
        }

        _consoleHelpers.Pause();
    }

    private async Task PrintDepartmentsAsync()
    {
        try
        {
            var departments = await _departmentRepository.GetDepartmentsAsync();
            if (departments.Count == 0)
            {
                Console.WriteLine("(no departments)");
                return;
            }

            foreach (var d in departments)
                Console.WriteLine($"ID: {d.DepartmentId} | {d.DepartmentName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to load departments: " + ex.Message);
        }
    }

  
}