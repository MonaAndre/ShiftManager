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
    
    public TimeSpan ReadTimeSpan(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (TimeSpan.TryParse(input, out var ts))
                return ts;

            Console.WriteLine("Invalid time format. Use HH:mm (example: 09:00).");
        }
    }
    public DateTime ReadFutureMondayDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (!DateTime.TryParseExact(
                    input,
                    new[] { "yyyy-MM-dd", "yyyy/MM/dd" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date))
            {
                Console.WriteLine("Invalid date format. Use yyyy-MM-dd (example: 2026-02-09).");
                continue;
            }

            date = date.Date;

            if (date.DayOfWeek != DayOfWeek.Monday)
            {
                Console.WriteLine("Date must be a Monday.");
                continue;
            }

            if (date <= DateTime.Today)
            {
                Console.WriteLine("Date must be in the future.");
                continue;
            }

            return DateTime.SpecifyKind(date, DateTimeKind.Local);
        }
    }


}