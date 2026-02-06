using ShiftManager.Domain.Interfaces;
using ShiftManager.Domain.Interfaces.Repositories;
using ShiftManager.Domain.Interfaces.Services;
using ShiftManager.Domain.Models;

namespace ShiftManager.Presentation;

public class SchedulingMenu
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ISchedulingService _schedulingService;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ConsoleHelpers _consoleHelpers;

    public SchedulingMenu(IDepartmentRepository departmentRepository, ISchedulingService schedulingService,
        IEmployeeRepository employeeRepository, ConsoleHelpers consoleHelpers)
    {
        _departmentRepository = departmentRepository;
        _schedulingService = schedulingService;
        _employeeRepository = employeeRepository;
        _consoleHelpers = consoleHelpers;
    }

    public async Task OpenSchedulingMenu()
    {
        Console.WriteLine("----------- Scheduling ------------");
        Console.WriteLine("1. Publish week schedule (one employee)");
        Console.WriteLine("2. Back");
        Console.WriteLine("----------------------------------");
    }

    public async Task RunSchedulingAsync()
    {
        while (true)
        {
            Console.Clear();
            await OpenSchedulingMenu();
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
                    await PublishWeek();
                    break;
                case 2:
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

    private async Task PublishWeek()
    {
        Console.Clear();
        Console.WriteLine("Schedule Week for an employee");
        Console.WriteLine("-------------------");
        try
        {
            await _departmentRepository.PrintDepartmentsAsync();

            var departmentId = _consoleHelpers.ReadInt("Enter department ID: ");
            var isValidDep = await _departmentRepository.IsValidDepIdAsync(departmentId.Value);
            if (!isValidDep) return;

            await _employeeRepository.PrintEmployeesByDepartmentAsync(departmentId.Value);
            var employeeId = _consoleHelpers.ReadInt("Enter employee ID: ");
            if (employeeId is null) return;
            var isValidEmployeeId =
                await _employeeRepository.IsValidEmployeeIdDepartmentAsync(employeeId.Value, departmentId.Value);
            if (!isValidEmployeeId) return;

            var startLocal = _consoleHelpers.ReadFutureMondayDate(
                "Enter week start date (yyyy-MM-dd, must be a future Monday): ");

            var weekStartUtc = startLocal.ToUniversalTime();
            var dayStartTime = _consoleHelpers.ReadTimeSpan("Enter day start time (HH:mm), e.g. 09:00: ");
            var dayEndTime = _consoleHelpers.ReadTimeSpan("Enter day end time (HH:mm), e.g. 17:00: ");

            var result = await _schedulingService.PublishWeekForEmployeeAsync(
                departmentId.Value,
                employeeId.Value,
                weekStartUtc,
                dayStartTime,
                dayEndTime);

            Console.WriteLine(result.Message);
            Console.WriteLine($"Created: {result.CreatedCount}");
            _consoleHelpers.Pause();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine("Failed to schedule week.");
            _consoleHelpers.Pause();
        }
    }
}