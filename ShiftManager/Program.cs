using ShiftManager.Data;
using ShiftManager.Data.Repositories;
using ShiftManager.Presentation;

namespace ShiftManager;

class Program
{
    static async Task Main(string[] args)
    {
        var appContext = new AppDbContext();
        var departmentRepo = new DepartmentRepository(appContext);
        var departmentMenu = new DepartmentMenu(departmentRepo);
        var menu = new MainMenu(departmentMenu);
        await menu.RunMainAsync();
    }
}