using ShiftManager.Data.Repositories;
using ShiftManager.Domain.Interfaces;

namespace ShiftManager.Presentation;

public class MainMenu
{
    private readonly DepartmentMenu _departmentMenu;
    private readonly EmployeeMenu _employeeMenu;
    private readonly RoleMenu _roleMenu;
    private readonly ShiftMenu _shiftMenu;

    public MainMenu(DepartmentMenu departmentMenu, EmployeeMenu employeeMenu, RoleMenu roleMenu, ShiftMenu shiftMenu)
    {
        _departmentMenu = departmentMenu;
        _employeeMenu = employeeMenu;
        _roleMenu = roleMenu;
        _shiftMenu = shiftMenu;
    }

    public void OpenMainMenu()
    {
        Console.WriteLine("-----------Welcome to ShiftManager ------------");
        Console.WriteLine("Menu");
        Console.WriteLine("1. Manage Departments");
        Console.WriteLine("2. Manage Employees");
        Console.WriteLine("3. Manage Roles");
        Console.WriteLine("4. Manage Shifts");
        Console.WriteLine("5. Reports");
        Console.WriteLine("6. Role assignments");
        Console.WriteLine("7. Scheduling");
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
                Console.WriteLine("Ogiltigt val. Ange en siffra.");
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
                    Console.WriteLine("Reports menu");
                    break;
                case 6:
                    Console.WriteLine("Role assignments");
                    break;
                case 7:
                    Console.WriteLine("Scheduling");
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