using ShiftManager.Domain.Interfaces;
using ShiftManager.Domain.Interfaces.Services;
using ShiftManager.Domain.Services;

namespace ShiftManager.Presentation;

public class ReportsMenu
{
    private readonly IReportsService _reportsService;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ConsoleHelpers _consoleHelpers;

    public ReportsMenu(ReportsService reportsService, IDepartmentRepository departmentRepository,
        ConsoleHelpers consoleHelpers)
    {
        _reportsService = reportsService;
        _departmentRepository = departmentRepository;
        _consoleHelpers = consoleHelpers;
    }

    private void OpenReportsMenu()
    {
        Console.WriteLine("Reports");
        Console.WriteLine("-----------------");
        Console.WriteLine("You can:");
        Console.WriteLine("1. Get department summary");
        Console.WriteLine("2. Back to main menu");
    }

    public async Task RunReportsAsync()
    {
        while (true)
        {
            Console.Clear();
            OpenReportsMenu();
            Console.WriteLine("Chose option 1-2");
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
                    await GetDepartmentSummary();
                    break;
                case 2:
                    return;
                default:
                {
                    Console.WriteLine("Invalid option");
                    break;
                }
            }
        }
    }

    private async Task GetDepartmentSummary()
    {
        try
        {
            await _departmentRepository.PrintDepartmentsAsync();
            var departmentId = _consoleHelpers.ReadInt("Enter department ID: ");
            if (departmentId is null) return;
            var isValidDep = await _departmentRepository.IsValidDepIdAsync(departmentId.Value);
            if (!isValidDep) return;
            await _reportsService.PrintDepartmentSummaryAsync(departmentId.Value);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine("Failed to get department summary.");
            _consoleHelpers.Pause();
        }
    }
}