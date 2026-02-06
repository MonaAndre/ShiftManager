using ShiftManager.Domain.Interfaces.Repositories;
using ShiftManager.Domain.Interfaces.Services;

namespace ShiftManager.Presentation;

public class RoleAssignmentMenu
{
    private readonly IRoleAssignmentService _roleAssignmentService;
    private readonly IRoleRepository _roleRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ConsoleHelpers _consoleHelpers;

    public RoleAssignmentMenu(IRoleAssignmentService roleAssignmentService, IRoleRepository roleRepository,
        IEmployeeRepository employeeRepository, ConsoleHelpers consoleHelpers)
    {
        _roleAssignmentService = roleAssignmentService;
        _roleRepository = roleRepository;
        _employeeRepository = employeeRepository;
        _consoleHelpers = consoleHelpers;
    }

    private void OpenRoleAssignmentMenu()
    {
        Console.WriteLine("----------- Role assignment ------------");
        Console.WriteLine("You can:");
        Console.WriteLine("1. List roles for an employee");
        Console.WriteLine("2. List all employees for a role");
        Console.WriteLine("3. Add Role for an employee");
        Console.WriteLine("4. Delete a role for an employee");
        Console.WriteLine("5. Back to main menu");
        Console.WriteLine("--------------------------------------------");
    }

    public async Task RunAssignmentAsync()
    {
        while (true)
        {
            Console.Clear();
            OpenRoleAssignmentMenu();
            Console.Write("Choose option 1-5: ");
            var input = Console.ReadLine()?.Trim();
            if (!int.TryParse(input, out var choice))
            {
                Console.Clear();
                Console.WriteLine("Invalid choice. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    await ListRolesForEmployee();
                    break;
                case 2:
                    await ListEmployeesForRole();
                    break;
                case 3:
                    await AddRoleToEmployee();
                    break;
                case 4:
                    await RemoveRoleFromEmployee();
                    break;
                case 5:
                    Console.Clear();
                    return;
                default:
                {
                    Console.Clear();
                    Console.WriteLine("Invalid option");
                    _consoleHelpers.Pause();
                    break;
                }
            }
        }
    }

    private async Task ListRolesForEmployee()
    {
        Console.Clear();
        Console.WriteLine("Listing all roles for an employee");
        try
        {
            var employees = await _employeeRepository.GetEmployeesAsync();
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees found");
                _consoleHelpers.Pause();
                return;
            }

            foreach (var employee in employees)
            {
                Console.WriteLine($"ID: {employee.EmployeeId} | {employee.FirstName} {employee.LastName}");
            }

            var employeeId = _consoleHelpers.ReadInt("Enter employee id: ");
            if (employeeId is null) return;
            var validEmployeeId = await _employeeRepository.IsValidEmployeeIdAsync(employeeId.Value);
            if (!validEmployeeId)
            {
                Console.WriteLine("Invalid employee id");
                _consoleHelpers.Pause();
                return;
            }

            var result = await _roleAssignmentService.GetRolesForEmployeeAsync(employeeId.Value);
            if (result is null)
            {
                Console.WriteLine("Could not get any role assignments for employee");
                _consoleHelpers.Pause();
                return;
            }

            Console.WriteLine($"Role assignment for {result.EmployeeName}");
            if (result.Roles.Count == 0)
            {
                Console.WriteLine("- No roles found");
                _consoleHelpers.Pause();
                return;
            }

            foreach (var role in result.Roles)
            {
                Console.WriteLine($" - {role.RoleId} | {role.RoleName}");
            }

            _consoleHelpers.Pause();
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to list roles for employee.");
            Console.WriteLine(e.Message);
            _consoleHelpers.Pause();
        }
    }

    private async Task ListEmployeesForRole()
    {
        Console.Clear();
        Console.WriteLine("Listing all employees for a role");

        try
        {
            await _roleRepository.PrintRolesAsync();

            var roleId = _consoleHelpers.ReadInt("Enter role id: ");
            if (roleId is null) return;

            var isValidRoleId = await _roleRepository.IsValidRoleIdAsync(roleId.Value);
            if (!isValidRoleId)
            {
                Console.WriteLine("Invalid role id.");
                _consoleHelpers.Pause();
                return;
            }

            var result = await _roleAssignmentService.GetEmployeesForRoleAsync(roleId.Value);

            Console.WriteLine();
            if (result is null)
            {
                Console.WriteLine("Could not get any employee for this role");
                _consoleHelpers.Pause();
                return;
            }

            if (result.Employees.Count == 0)
            {
                Console.WriteLine("No employees assigned to this role.");
                _consoleHelpers.Pause();
                return;
            }

            Console.WriteLine($"Employees that have role: {result.RoleName}");
            foreach (var employee in result.Employees)
            {
                Console.WriteLine(
                    $"ID: {employee.EmployeeId} | {employee.FirstName} {employee.LastName}");
            }

            _consoleHelpers.Pause();
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to list employees for role.");
            Console.WriteLine(e.Message);
            _consoleHelpers.Pause();
        }
    }

    private async Task AddRoleToEmployee()
    {
        Console.Clear();
        Console.WriteLine("Adding a role to an employee");
        try
        {
            var employees = await _employeeRepository.GetEmployeesAsync();
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees found");
                _consoleHelpers.Pause();
                return;
            }

            foreach (var employee in employees)
            {
                Console.WriteLine($"ID: {employee.EmployeeId} | {employee.FirstName} {employee.LastName}");
            }

            var employeeId = _consoleHelpers.ReadInt("Enter employee id: ");
            if (employeeId is null) return;
            var validEmployeeId = await _employeeRepository.IsValidEmployeeIdAsync(employeeId.Value);

            if (!validEmployeeId)
            {
                Console.WriteLine("Invalid employee id");
                _consoleHelpers.Pause();
                return;
            }

            await _roleRepository.PrintRolesAsync();

            var roleId = _consoleHelpers.ReadInt("Enter role id: ");
            if (roleId is null) return;

            var isValidRoleId = await _roleRepository.IsValidRoleIdAsync(roleId.Value);
            if (!isValidRoleId)
            {
                Console.WriteLine("Invalid role id.");
                _consoleHelpers.Pause();
                return;
            }

            var result = await _roleAssignmentService.AssignRoleAsync(employeeId.Value, roleId.Value);
            if (!result)
            {
                Console.WriteLine("Could not add role to employee.");
                _consoleHelpers.Pause();
                return;
            }

            Console.WriteLine("Role assignment added");

            _consoleHelpers.Pause();
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to add role to employee.");
            Console.WriteLine(e);
            _consoleHelpers.Pause();
        }
    }

    private async Task RemoveRoleFromEmployee()
    {
        Console.Clear();
        Console.WriteLine("Removing a role from an employee");

        try
        {
            var employees = await _employeeRepository.GetEmployeesAsync();
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees found");
                _consoleHelpers.Pause();
                return;
            }

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.EmployeeId} | {employee.FirstName} {employee.LastName}");
            }

            var employeeId = _consoleHelpers.ReadInt("Enter employee id: ");
            if (employeeId is null) return;

            var validEmployeeId = await _employeeRepository.IsValidEmployeeIdAsync(employeeId.Value);
            if (!validEmployeeId)
            {
                Console.WriteLine("Invalid employee id");
                _consoleHelpers.Pause();
                return;
            }

            var employeeRoles = await _roleAssignmentService.GetRolesForEmployeeAsync(employeeId.Value);
            if (employeeRoles is null)
            {
                Console.WriteLine("Employee not found.");
                _consoleHelpers.Pause();
                return;
            }

            if (employeeRoles.Roles.Count == 0)
            {
                Console.WriteLine($"{employeeRoles.EmployeeName} has no roles assigned.");
                _consoleHelpers.Pause();
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"Roles for {employeeRoles.EmployeeName}:");


            foreach (var role in employeeRoles.Roles)
            {
                Console.WriteLine($"{role.RoleId} | {role.RoleName}");
            }

            var roleId = _consoleHelpers.ReadInt("Enter role id to remove: ");
            if (roleId is null) return;

            var employeeHasRole = employeeRoles.Roles.Any(r => r.RoleId == roleId.Value);
            if (!employeeHasRole)
            {
                Console.WriteLine("Invalid role id (employee does not have that role).");
                _consoleHelpers.Pause();
                return;
            }

            var removed = await _roleAssignmentService.RemoveRoleAsync(employeeId.Value, roleId.Value);

            if (!removed)
            {
                Console.WriteLine("Could not remove role (it may already have been removed).");
                _consoleHelpers.Pause();
                return;
            }

            Console.WriteLine("Role removed successfully.");
            _consoleHelpers.Pause();
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to remove role.");
            Console.WriteLine(e.Message);
            _consoleHelpers.Pause();
        }
    }
}