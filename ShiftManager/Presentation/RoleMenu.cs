using ShiftManager.Domain.Interfaces.Repositories;
using ShiftManager.Domain.Models;

namespace ShiftManager.Presentation;

public class RoleMenu
{
    private readonly IRoleRepository _roleRepository;
    private readonly ConsoleHelpers _consoleHelpers;

    public RoleMenu(IRoleRepository roleRepository, ConsoleHelpers consoleHelpers)
    {
        _roleRepository = roleRepository;
        _consoleHelpers = consoleHelpers;
    }

    private void OpenRoleMenu()
    {
        Console.WriteLine("----------- Roles management ------------");
        Console.WriteLine("You can:");
        Console.WriteLine("1. List roles");
        Console.WriteLine("2. Add role");
        Console.WriteLine("3. Update role");
        Console.WriteLine("4. Delete role");
        Console.WriteLine("5. Back to main menu");
        Console.WriteLine("--------------------------------------------");
    }

    public async Task RunRoleAsync()
    {
        while (true)
        {
            Console.Clear();
            OpenRoleMenu();
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

                    await ListRoles();

                    break;
                case 2:
                    await AddRole();
                    break;
                case 3:
                    await EditRole();
                    break;
                case 4:
                    await DeleteRole();
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

    private async Task ListRoles()
    {
        Console.Clear();
        Console.WriteLine("List Roles");
        try
        {
            var roleslList = await _roleRepository.GetRolesAsync();
            if (roleslList.Count == 0)
            {
                Console.WriteLine("No role found");
                _consoleHelpers.Pause();
                return;
            }

            foreach (var r in roleslList)
            {
                Console.Write("ID: " + r.RoleId + " | Name: ");
                Console.Write(r.RoleName);
                Console.Write(" | Description: " + r.RoleDescription);
                Console.WriteLine();
            }

            Console.WriteLine();
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to get roles :" + e);
        }

        _consoleHelpers.Pause();
    }

    private async Task AddRole()
    {
        Console.Clear();
        Console.WriteLine("Add role");
        var name = _consoleHelpers.ReadNonEmptyString("Name: ");
        if (name is null) return;
        var description = _consoleHelpers.ReadNonEmptyString("Description: ");
        if (description is null) return;
        try
        {
            var role = new Role
            {

                RoleName = name,
                RoleDescription = description
            };
            var newRole = await _roleRepository.CreateRoleAsync(role);
            Console.WriteLine(
                "New role added: " + "ID: " + newRole.RoleId + " | name: " +
                newRole.RoleName + " | description: " + newRole.RoleDescription);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        _consoleHelpers.Pause();
    }

    private async Task EditRole()
    {
        Console.Clear();
        Console.WriteLine("Edite role");
        try
        {
            await _roleRepository.PrintRolesAsync();
            var id = _consoleHelpers.ReadInt("Enter id of role that you want to edite: ");
            if (id is null) return;
            var isValidId = await _roleRepository.IsValidRoleIdAsync(id.Value);
            if (!isValidId)
            {
                Console.WriteLine("Could not find role with this id");
                _consoleHelpers.Pause();
                return;
            }

            var newRoleName = _consoleHelpers.ReadNonEmptyString("New name: ");
            if (newRoleName is null) return;
            var newDescriptionName = _consoleHelpers.ReadNonEmptyString("New description: ");
            if (newDescriptionName is null) return;
            var editedRole = await _roleRepository.UpdateRoleAsync(id.Value, newRoleName, newDescriptionName);
            if (editedRole is null)
            {
                Console.WriteLine("Could not update role with this id");
                _consoleHelpers.Pause();
                return;
            }

            Console.WriteLine("Role was updated: " + "ID: " + editedRole.RoleId + " New name: " +
                              editedRole?.RoleName + " | description: " + editedRole?.RoleDescription);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        _consoleHelpers.Pause();
    }

    private async Task DeleteRole()
    {
        Console.Clear();
        Console.WriteLine("Delete role");
        try
        {
            await _roleRepository.PrintRolesAsync();
            var id = _consoleHelpers.ReadInt("Enter id of role that you want to delete: ");
            if (id is null) return;
            var isValidId = await _roleRepository.IsValidRoleIdAsync(id.Value);
            if (!isValidId)
            {
                Console.WriteLine("Could not find role with this id");
                _consoleHelpers.Pause();
                return;
            }

            var isDeleted = await _roleRepository.DeleteRoleAsync(id.Value);
            if (!isDeleted)
            {
                Console.WriteLine("Role was not deleted");
                _consoleHelpers.Pause();
                return;
            }


            Console.WriteLine("Role was deleted: " + "ID: " + id.Value);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        _consoleHelpers.Pause();
    }
}