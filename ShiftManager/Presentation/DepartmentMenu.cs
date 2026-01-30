using ShiftManager.Domain.Interfaces;
using ShiftManager.Domain.Models;

namespace ShiftManager.Presentation;

public class DepartmentMenu
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentMenu(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    private void OpenDepartmentMenu()
    {
        // Console.Clear();
        Console.WriteLine("----------- Departments management ------------");
        Console.WriteLine("You can:");
        Console.WriteLine("1. List departments");
        Console.WriteLine("2. Add new department");
        Console.WriteLine("3. Update department");
        Console.WriteLine("4. Delete department");
        Console.WriteLine("5. Back to main menu");
    }

    public async Task RunDepartmentAsync()
    {
        while (true)
        {
            OpenDepartmentMenu();
            Console.Write("Chose option 1-5: ");
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

                    await ListDepartments();

                    break;
                case 2:
                    await AddDepartment();
                    break;
                case 3:
                    await EditeDepartment();
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
                    return;
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
    }

    private async Task AddDepartment()
    {
        Console.Clear();
        Console.WriteLine("Add department");
        Console.Write("Enter name and press enter to create new department: ");
        var name = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name can not be empty");
            return;
        }

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
            throw;
        }
    }

    private async Task EditeDepartment()
    {
        Console.Clear();
        Console.WriteLine("Edite department");
        Console.Write("Enter id of department that you want to edite and press enter: ");
        var input = Console.ReadLine()?.Trim();
        if (!int.TryParse(input, out var id))
        {
            Console.WriteLine("Id must be an integer");
            return;
        }

        var isValidId = await _departmentRepository.IsValidDepId(id);
        if (!isValidId)
        {
            Console.WriteLine("Could not find department with this id");
            return;
        }

        Console.Write("Enter new name: ");
        var newDepName = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(newDepName))
        {
            Console.WriteLine("Name can not be empty");
            return;
        }

        try
        {
            var editedDepartment = await _departmentRepository.UpdateDepartmentAsync(id, newDepName);
            Console.WriteLine("Department was updated: " + "ID: " + editedDepartment.DepartmentId + " New name: " +
                              editedDepartment.DepartmentName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }

    private async Task DeleteDepartment()
    {
        Console.Clear();
        Console.WriteLine("Delete department");
        Console.Write("Enter id of department that you want to delete: ");
        var input = Console.ReadLine()?.Trim();
        if (!int.TryParse(input, out var departmentId))
        {
            Console.WriteLine("Not a valid id");
            return;
        }

        var isValidId = await _departmentRepository.IsValidDepId(departmentId);
        if (!isValidId)
        {
            Console.WriteLine("Could not find department with this id");
            return;
        }

        try
        {
            var isDeleted = await _departmentRepository.DeleteDepartmentAsync(departmentId);
            if (!isDeleted)
            {
                Console.WriteLine("Department was not deleted");
            }

            Console.WriteLine("Department was deleted: " + "ID: " + departmentId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}