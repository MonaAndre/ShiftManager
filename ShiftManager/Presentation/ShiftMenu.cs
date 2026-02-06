using System.Globalization;
using ShiftManager.Domain.Interfaces;
using ShiftManager.Domain.Interfaces.Repositories;
using ShiftManager.Domain.Models;

namespace ShiftManager.Presentation;

public class ShiftMenu
{
    private readonly IShiftRepository _shiftRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ConsoleHelpers _consoleHelpers;

    public ShiftMenu(IShiftRepository shiftRepository, IDepartmentRepository departmentRepository,
        IEmployeeRepository employeeRepository,
        ConsoleHelpers consoleHelpers)
    {
        _shiftRepository = shiftRepository;
        _departmentRepository = departmentRepository;
        _employeeRepository = employeeRepository;
        _consoleHelpers = consoleHelpers;
    }

    private void OpenShitsMenu()
    {
        Console.WriteLine("----------- Shifts management ------------");
        Console.WriteLine("You can:");
        Console.WriteLine("1. List shifts");
        Console.WriteLine("2. Show shift details");
        Console.WriteLine("3. Add shift");
        Console.WriteLine("4. Update shift");
        Console.WriteLine("5. Delete shift");
        Console.WriteLine("6. Back to main menu");
        Console.WriteLine("--------------------------------------------");
    }

    public async Task RunShiftAsync()
    {
        while (true)
        {
            Console.Clear();
            OpenShitsMenu();
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

                    await ListShifts();
                    break;
                case 2:
                    await ViewShiftDetails();
                    break;
                case 3:
                    await AddShift();
                    break;
                case 4:
                    await EditShift();
                    break;
                case 5:
                    await DeleteShift();
                    break;
                case 6:
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

    private async Task ListShifts()
    {
        Console.Clear();
        Console.WriteLine("List shifts");
        try
        {
            var shiftsList = await _shiftRepository.GetShiftsAsync();
            if (!shiftsList.Any())
            {
                Console.WriteLine("No shift found");
                _consoleHelpers.Pause();
                return;
            }

            foreach (var s in shiftsList)
            {
                PrintShift(s);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to get shifts");
        }

        _consoleHelpers.Pause();
    }

    private async Task ViewShiftDetails()
    {
        Console.Clear();
        Console.WriteLine("View shift details");
        var id = _consoleHelpers.ReadInt("Enter shift id: ");
        if (id is null) return;
        try
        {
            var existShift = await _shiftRepository.IsValidShiftIdAsync(id.Value);
            if (!existShift)
            {
                Console.WriteLine("Could not find shift with this id.");
                _consoleHelpers.Pause();
                return;
            }

            var shift = await _shiftRepository.FindShiftAsync(id.Value);
            if (shift is null)
            {
                Console.WriteLine("Could not load shift details.");
                _consoleHelpers.Pause();
                return;
            }

            PrintShiftDetails(shift);
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to get employee details");
        }

        _consoleHelpers.Pause();
    }

    private async Task AddShift()
    {
        Console.Clear();
        Console.WriteLine("Add shift");
        try
        {
            Console.WriteLine("Chose department");
            await _departmentRepository.PrintDepartmentsAsync();

            var id = _consoleHelpers.ReadInt("Enter department id: ");
            if (id is null) return;
            var isValidDepId = await _departmentRepository.IsValidDepIdAsync(id.Value);
            if (!isValidDepId)
            {
                Console.WriteLine("Chosen department id is invalid.");
                _consoleHelpers.Pause();
                return;
            }

            Console.WriteLine("Chose employee");
            var isAnyEmployee = await _employeeRepository.PrintEmployeesByDepartmentAsync(id.Value);
            if (!isAnyEmployee)
            {
                _consoleHelpers.Pause();
                return;
            }

            var employeeId = _consoleHelpers.ReadInt("Enter employee id: ");
            var isValidEmployee =
                await _employeeRepository.IsValidEmployeeIdDepartmentAsync(employeeId!.Value, id.Value);
            if (!isValidEmployee)
            {
                Console.WriteLine("Chosen employee id is invalid.");
                _consoleHelpers.Pause();
                return;
            }

            var start = _consoleHelpers.ReadDateTime("Enter start date and time (yyyy-MM-dd HH:mm)");


            if (start is null) return;

            if (start.Value < DateTime.UtcNow)
            {
                Console.WriteLine("Start date cannot be in the past.");
                _consoleHelpers.Pause();
                return;
            }

            var end = _consoleHelpers.ReadDateTime("Enter end date and time (yyyy-MM-dd HH:mm)");
            if (end is null) return;
            if (end.Value < start.Value)
            {
                Console.WriteLine("End date and time must be later than start date.");
                _consoleHelpers.Pause();
                return;
            }


            var isOverLapping =
                await _shiftRepository.HasOverlappingShiftAsync(employeeId.Value, id.Value, start.Value, end.Value);
            if (isOverLapping)
            {
                Console.WriteLine("Employee has overlapping shift. Change time for new shift");
                _consoleHelpers.Pause();
                return;
            }

            var newShift = new Shift
            {
                EmployeeId = employeeId.Value,
                DepartmentId = id.Value,
                StartDate = start.Value,
                EndDate = end.Value
            };

            var result = await _shiftRepository.CreateShiftAsync(newShift);
            if (result is null)
            {
                Console.WriteLine("Could not create new shift.");
                _consoleHelpers.Pause();
                return;
            }

            Console.WriteLine($"Shift created with ID: {result.ShiftId}");
            _consoleHelpers.Pause();
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to add a shift");
            _consoleHelpers.Pause();
            return;
        }
    }

    private async Task EditShift()
    {
        Console.Clear();
        Console.WriteLine("EDIT SHIFT");
        Console.WriteLine("You can only edit feature shifts");

        try
        {
            var anyShifts = await _shiftRepository.PrintFutureShiftsAsync();
            if (!anyShifts)
            {
                _consoleHelpers.Pause();
                return;
            }

            var shiftId = _consoleHelpers.ReadInt("Enter shift id to edit: ");
            if (shiftId is null) return;

            var shift = await _shiftRepository.FindShiftAsync(shiftId.Value);
            if (shift is null)
            {
                Console.WriteLine("Shift not found.");
                _consoleHelpers.Pause();
                return;
            }

            if (shift.StartDate <= DateTime.UtcNow)
            {
                Console.WriteLine("That shift is in the past and cannot be edited. Choose a future shift.");
                _consoleHelpers.Pause();
                return;
            }

            Console.WriteLine(
                $"Current: {shift.StartDate.ToLocalTime():yyyy-MM-dd HH:mm} -> {shift.EndDate.ToLocalTime():yyyy-MM-dd HH:mm}");

            var start = _consoleHelpers.ReadDateTime("Enter new start date and time (yyyy-MM-dd HH:mm): ");
            if (start is null) return;

            if (start.Value < DateTime.UtcNow)
            {
                Console.WriteLine(start);
                Console.WriteLine("Start date cannot be in the past.");
                _consoleHelpers.Pause();
                return;
            }


            var end = _consoleHelpers.ReadDateTime("Enter new end date and time (yyyy-MM-dd HH:mm): ");
            if (end is null) return;


            if (end.Value <= start.Value)
            {
                Console.WriteLine("End date and time must be later than start date.");
                _consoleHelpers.Pause();
                return;
            }


            var updated = await _shiftRepository.UpdateShiftAsync(shiftId.Value, start.Value, end.Value);

            if (updated is null)
            {
                Console.WriteLine("Could not update shift (overlapping shift or invalid data).");
                _consoleHelpers.Pause();
                return;
            }

            Console.WriteLine("Shift updated successfully.");
            _consoleHelpers.Pause();
        }
        catch
        {
            Console.WriteLine("Failed to edit shift.");
            _consoleHelpers.Pause();
        }
    }

    private async Task DeleteShift()
    {
        Console.Clear();
        Console.WriteLine("Delete shift");

        try
        {
            var anyShifts = await _shiftRepository.PrintFutureShiftsAsync();
            if (!anyShifts)
            {
                _consoleHelpers.Pause();
                return;
            }

            var shiftId = _consoleHelpers.ReadInt("Enter shift id to delete: ");
            if (shiftId is null) return;
          

            var shift = await _shiftRepository.FindShiftAsync(shiftId.Value);
            if (shift is null)
            {
                Console.WriteLine("Shift not found.");
                _consoleHelpers.Pause();
                return;
            }
            if (shift.StartDate <= DateTime.UtcNow)
            {
                Console.WriteLine("That shift is in the past and cannot be deleted here.");
                _consoleHelpers.Pause();
                return;
            }
            Console.WriteLine(
                $"Delete shift {shift.ShiftId}: " +
                $"{shift.StartDate.ToLocalTime():yyyy-MM-dd HH:mm} → " +
                $"{shift.EndDate.ToLocalTime():yyyy-MM-dd HH:mm}? (y/n)");

            var confirm = Console.ReadLine();
            if (!string.Equals(confirm, "y", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Delete cancelled.");
                _consoleHelpers.Pause();
                return;
            }

            var deleted = await _shiftRepository.DeleteShiftAsync(shiftId.Value);
            if (!deleted)
            {
                Console.WriteLine("Could not delete shift.");
                _consoleHelpers.Pause();
                return;
            }

            Console.WriteLine("Shift deleted successfully.");
            _consoleHelpers.Pause();
        }
        catch
        {
            Console.WriteLine("Failed to delete shift.");
            _consoleHelpers.Pause();
        }
    }

    private static void PrintShiftDetails(Shift shift)
    {
        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Shift ID   : {shift.ShiftId}");
        Console.WriteLine($"Employee   : {shift.Employee.FirstName} {shift.Employee.LastName}");
        Console.WriteLine($"Department : {shift.Department.DepartmentName}");
        Console.WriteLine($"Start      : {shift.StartDate.ToLocalTime():yyyy-MM-dd HH:mm}");
        Console.WriteLine($"End        : {shift.EndDate.ToLocalTime():yyyy-MM-dd HH:mm}");
        Console.WriteLine("----------------------------------------");
    }

    private static void PrintShift(Shift shift)
    {
        Console.WriteLine(
            $"[{shift.ShiftId}] " +
            $"{shift.StartDate.ToLocalTime():yyyy-MM-dd HH:mm} → {shift.EndDate.ToLocalTime():HH:mm} | " +
            $"{shift.Department.DepartmentName}"
        );
    }
}