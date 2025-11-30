namespace VendingMachineApp.Utils;

public static class ConsoleUI
{
    public static void Pause(string? msg = null)
    {
        if (!string.IsNullOrWhiteSpace(msg))
            Console.WriteLine(msg);
        Console.WriteLine("Нажмите Enter...");
        Console.ReadLine();
    }

    public static void Clear() => Console.Clear();
}