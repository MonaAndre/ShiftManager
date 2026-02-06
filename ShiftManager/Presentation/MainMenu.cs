using ShiftManager.Data.Repositories;
using ShiftManager.Domain.Interfaces;
using ShiftManager.Domain.Services;

namespace ShiftManager.Presentation;

public class MainMenu
{
    private readonly DepartmentMenu _departmentMenu;
    private readonly EmployeeMenu _employeeMenu;
    private readonly RoleMenu _roleMenu;
    private readonly ShiftMenu _shiftMenu;
    private readonly RoleAssignmentMenu _roleAssignmentMenu;
    private readonly SchedulingMenu _schedulingMenu;
    private readonly ReportsMenu _reportsMenu;

    public MainMenu(DepartmentMenu departmentMenu, EmployeeMenu employeeMenu, RoleMenu roleMenu, ShiftMenu shiftMenu,
        RoleAssignmentMenu roleAssignmentMenu, SchedulingMenu schedulingMenu, ReportsMenu reportsMenu)
    {
        _departmentMenu = departmentMenu;
        _employeeMenu = employeeMenu;
        _roleMenu = roleMenu;
        _shiftMenu = shiftMenu;
        _roleAssignmentMenu = roleAssignmentMenu;
        _schedulingMenu= schedulingMenu;
        _reportsMenu= reportsMenu;
    }

    public void OpenMainMenu()
    {
        Console.WriteLine("-----------Welcome to ShiftManager ------------");
        Console.WriteLine("Menu");
        Console.WriteLine("1. Manage Departments");
        Console.WriteLine("2. Manage Employees");
        Console.WriteLine("3. Manage Roles");
        Console.WriteLine("4. Manage Shifts");
        Console.WriteLine("5. Scheduling");
        Console.WriteLine("6. Role assignments");
        Console.WriteLine("7. Reports");
        Console.WriteLine("8. Exit");
        Console.WriteLine("--------------------------------------------");
    }

    public async Task RunMainAsync()
    {
        while (true)
        {
            OpenMainMenu();
            Console.Write("Chose option 1-8:    ");
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
                    await _departmentMenu.RunDepartmentAsync();
                    break;
                case 2:
                    await _employeeMenu.RunEmployeeAsync();
                    break;
                case 3:
                    await _roleMenu.RunRoleAsync();
                    break;
                case 4:
                    await _shiftMenu.RunShiftAsync();
                    break;
                case 5:
                    await _schedulingMenu.RunSchedulingAsync();
                    break;
                case 6:
                    await _roleAssignmentMenu.RunAssignmentAsync();
                    break;
                case 7:
                   await _reportsMenu.RunReportsAsync();
                    break;
                case 8:
                    return;
                default:
                {
                    Console.WriteLine("Invalid option");
                    break;
                }
            }
        }
    }
}