
using ShiftManager.Domain;

namespace ShiftManager;

class Program
{
    static async Task Main(string[] args)
    {
        var app = new ShiftManagerApp();
        await app.RunAsync();
    }
}