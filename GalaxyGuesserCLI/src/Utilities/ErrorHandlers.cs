using System;

namespace ConsoleApp1.Utilities
{
    public static class ErrorHandler
    {
        public static void HandleError(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"🚨 Error: {ex.Message}");
            Console.ResetColor();
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}