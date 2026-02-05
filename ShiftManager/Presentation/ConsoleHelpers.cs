using System.Globalization;

namespace ShiftManager.Presentation;

public class ConsoleHelpers
{
    public int? ReadInt(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();

        if (!int.TryParse(input, out var value))
        {
            Console.WriteLine("Invalid choice. Please enter a number.");
            Pause();
            return null;
        }

        return value;
    }

    public string? ReadNonEmptyString(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Value cannot be empty.");
            Pause();
            return null;
        }

        return input;
    }

    public DateTime? ReadDateTime(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();
        if (!DateTime.TryParseExact(input,"yyyy-MM-dd HH:mm",CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AdjustToUniversal,out var utcDate))
        {
            Console.WriteLine("Invalid date. Please use format: yyyy-MM-dd HH:mm");
            Pause();
            return null;
        }
        return utcDate;
    }

    public void Pause()
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
}