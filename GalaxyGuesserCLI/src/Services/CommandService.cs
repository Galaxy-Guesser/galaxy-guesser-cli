using System;
using System.Collections.Generic;
using System.Threading;
using ConsoleApp1.Models;
using ConsoleApp1.Data;

namespace ConsoleApp1.Services
{
    public class CommandService
    {
        public static readonly string CMD_PREFIX = "/";
        private static readonly Dictionary<string, string> COMMANDS = UIData.COMMANDS;

        internal static void ProcessCommand(string command, Player player)
        {
            switch (command)
            {
                case "help":
                    UIService.ShowHelp(COMMANDS);
                    break;
                case "categories":
                    ViewCategories();
                    break;
                case "sessions":
                    ViewSessions();
                    break;
                case "howtoplay":
                    UIService.ShowHowToPlay();
                    break;
                case "leaderboard":
                    ViewLeaderboard();
                    break;
                case "myprofile":
                    ViewProfile(player);
                    break;
                case "mysessions":
                    ViewMySessionHistory(player);
                    break;
                case "quit":
                    Console.WriteLine("\n👋 Thanks for playing Galaxy Quiz! See you among the stars!");
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                    break;
                case "stats":
                    ViewGameStats();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Unknown command: {command}");
                    Console.WriteLine("Type '/help' to see available commands.");
                    Console.ResetColor();
                    break;
            }
        }

        // Command implementations (stubs for now)
        private static void ViewCategories()
        {
            Console.WriteLine("\nCategories:");
            foreach (var category in SampleData.Categories)
            {
                Console.WriteLine($"- {category.Name}");
            }
        }

        private static void ViewSessions()
        {
            Console.WriteLine("\nThis feature is not yet implemented.");
        }

        private static void ViewLeaderboard()
        {
            Console.WriteLine("\nThis feature is not yet implemented.");
        }

        private static void ViewProfile(Player player)
        {
            Console.WriteLine($"\nProfile for {player.Name}:");
            Console.WriteLine($"Username: {player.Username}");
            Console.WriteLine($"Player ID: {player.Id}");
            Console.WriteLine($"GUID: {player.Guid}");
            // You could add more stats here like games played, win ratio, etc.
        }

        private static void ViewMySessionHistory(Player player)
        {
            Console.WriteLine("\nThis feature is not yet implemented.");
        }

        private static void ViewGameStats()
        {
            Console.WriteLine("\nThis feature is not yet implemented.");
        }

        public static bool IsCommand(string input)
        {
            return input.StartsWith(CMD_PREFIX);
        }

        public static string ExtractCommandName(string input)
        {
            return input.Substring(1).ToLower();
        }
    }
}