using ShiftManager.Domain.Interfaces;
using ShiftManager.Domain.Models;

namespace ShiftManager.Presentation;

public class DepartmentMenu
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ConsoleHelpers _consoleHelpers;

    public DepartmentMenu(IDepartmentRepository departmentRepository, ConsoleHelpers consoleHelpers)
    {
        _departmentRepository = departmentRepository;
        _consoleHelpers = consoleHelpers;
    }

    private void OpenDepartmentMenu()
    {
        Console.WriteLine("----------- Department management ------------");
        Console.WriteLine("You can:");
        Console.WriteLine("1. List departments");
        Console.WriteLine("2. Add new department");
        Console.WriteLine("3. Update department");
        Console.WriteLine("4. Delete department");
        Console.WriteLine("5. Back to main menu");
        Console.WriteLine("--------------------------------------------");
    }

    public async Task RunDepartmentAsync()
    {
        while (true)
        {
            Console.Clear();
            OpenDepartmentMenu();
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

                    await ListDepartments();

                    break;
                case 2:
                    await AddDepartment();
                    break;
                case 3:
                    await EditDepartment();
                    break;
                case 4:
                    await DeleteDepartment();
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

    private async Task ListDepartments()
    {
        Console.Clear();
        Console.WriteLine("List departments");
        try
        {
            var departmentsList = await _departmentRepository.GetDepartmentsAsync();
            if (departmentsList.Count == 0)
            {
                Console.WriteLine("No departments found");
                _consoleHelpers.Pause();
                return;
            }

            foreach (var d in departmentsList)
            {
                Console.Write("ID: " + d.DepartmentId + " | Name: ");
                Console.Write(d.DepartmentName);
                Console.WriteLine();
            }

            Console.WriteLine();
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to get departments :" + e);
        }

        _consoleHelpers.Pause();
    }

    private async Task AddDepartment()
    {
        Console.Clear();
        Console.WriteLine("Add department");
        var name = _consoleHelpers.ReadNonEmptyString("Enter name and press enter to create new department: ");
        if (name is null) return;

        try
        {
            var createdDepartment = await _departmentRepository.CreateNewDepartmentAsync(name);
            Console.WriteLine(
                "New department added: " + "ID: " + createdDepartment.DepartmentId + " | name: " +
                createdDepartment.DepartmentName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        _consoleHelpers.Pause();
    }

    private async Task EditDepartment()
    {
        Console.Clear();
        Console.WriteLine("Edite department");
        var id = _consoleHelpers.ReadInt("Enter id of department that you want to edite and press enter: ");
        if (id is null) return;
        try
        {
            var isValidId = await _departmentRepository.IsValidDepIdAsync(id.Value);
            if (!isValidId)
            {
                Console.WriteLine("Could not find department with this id");
                _consoleHelpers.Pause();
                return;
            }

            var newDepName = _consoleHelpers.ReadNonEmptyString("New name: ");
            if (newDepName is null) return;
            var editedDepartment = await _departmentRepository.UpdateDepartmentAsync(id.Value, newDepName);
            if (editedDepartment is null)
            {
                Console.WriteLine("Could not update department with this id");
                _consoleHelpers.Pause();
                return;
            }

            Console.WriteLine("Department was updated: " + "ID: " + editedDepartment?.DepartmentId + " New name: " +
                              editedDepartment?.DepartmentName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        _consoleHelpers.Pause();
    }

    private async Task DeleteDepartment()
    {
        Console.Clear();
        Console.WriteLine("Delete department");
        var departmentId = _consoleHelpers.ReadInt("Enter id of department that you want to delete: ");
        if (departmentId is null) return;

        try
        {
            var isValidId = await _departmentRepository.IsValidDepIdAsync(departmentId.Value);
            if (!isValidId)
            {
                Console.WriteLine("Could not find department with this id");
                _consoleHelpers.Pause();
                return;
            }

            var isDeleted = await _departmentRepository.DeleteDepartmentAsync(departmentId.Value);
            if (!isDeleted)
            {
                Console.WriteLine("Department was not deleted");
                _consoleHelpers.Pause();
                return;
            }


            Console.WriteLine("Department was deleted: " + "ID: " + departmentId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        _consoleHelpers.Pause();
    }
}