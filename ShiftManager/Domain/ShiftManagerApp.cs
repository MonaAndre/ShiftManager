using ShiftManager.Data;
using ShiftManager.Data.Repositories;
using ShiftManager.Presentation;

namespace ShiftManager.Domain;

public class ShiftManagerApp
{
    public async Task RunAsync()
    {
        var appContext = new AppDbContext();
        var departmentRepo = new DepartmentRepository(appContext);
        var consoleHelpers = new ConsoleHelpers();
        var departmentMenu = new DepartmentMenu(departmentRepo, consoleHelpers);
        var roleRepo = new RoleRepository(appContext);
        var roleMenu = new RoleMenu(roleRepo, consoleHelpers);
        var employeeRepo = new EmployeeRepository(appContext);
        var employeeMenu = new EmployeeMenu(employeeRepo, departmentRepo, consoleHelpers);
        var shiftRepo = new ShiftRepository(appContext);
        var shiftMenu = new ShiftMenu(shiftRepo, departmentRepo,employeeRepo, consoleHelpers);

        var menu = new MainMenu(departmentMenu, employeeMenu, roleMenu, shiftMenu);
        await menu.RunMainAsync();
    }
}