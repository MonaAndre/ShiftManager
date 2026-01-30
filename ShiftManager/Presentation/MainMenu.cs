using ShiftManager.Data.Repositories;
using ShiftManager.Domain.Interfaces;

namespace ShiftManager.Presentation;

public class MainMenu
{
    private readonly DepartmentMenu _departmentMenu;

    public MainMenu(DepartmentMenu departmentMenu)
    {
        _departmentMenu = departmentMenu;
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
                    Console.WriteLine("Manage employee menu");
                    break;
                case 3:
                    Console.WriteLine("Manage Roles menu");
                    break;
                case 4:
                    Console.WriteLine("Manage Shifts menu");
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