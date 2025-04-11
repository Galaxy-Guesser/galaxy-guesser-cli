// src/Services/UIService.cs
using System;
using ConsoleApp1.Models;

namespace ConsoleApp1.Services
{
    public static class UIService
    {
        private const string CMD_PREFIX = "/";
        private static readonly (string Key, string Description)[] COMMANDS = new[]
        {
            ("help", "Show help information"),
            ("howtoplay", "Show how to play instructions"),
            ("categories", "List available categories"),
            ("sessions", "List or join quiz sessions"),
            ("quit", "Exit the application")
        };

        public static void ShowHowToPlay()
        {
            Console.Clear();
            PrintGalaxyHeader();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n🚀 HOW TO PLAY GALAXY QUIZ");
            Console.ResetColor();

            Console.WriteLine("\n1️⃣ CREATE OR JOIN A SESSION");
            Console.WriteLine("   • Create a new quiz session by selecting a category, question count, and time limit");
            Console.WriteLine("   • Or join an existing session by entering the session code provided by another player");

            Console.WriteLine("\n2️⃣ ANSWER THE QUESTIONS");
            Console.WriteLine("   • Each question has multiple-choice answers (A, B, C, D)");
            Console.WriteLine("   • Press the letter key corresponding to your answer");
            Console.WriteLine("   • Answer correctly to earn points");
            Console.WriteLine("   • Answer quickly to earn time bonus points (quicker = more bonus points)");

            Console.WriteLine("\n3️⃣ SCORING SYSTEM");
            Console.WriteLine("   • 1 point for each correct answer");
            Console.WriteLine("   • Bonus points for remaining time (1 point per second)");
            Console.WriteLine("   • Final score = correct answers + time bonus");

            Console.WriteLine("\n4️⃣ LEADERBOARDS");
            Console.WriteLine("   • Compare your scores with other players");
            Console.WriteLine("   • View your statistics and session history");

            Console.WriteLine("\n5️⃣ COMMANDS");
            Console.WriteLine("   • Use commands like '/help', '/categories', '/sessions' anytime");
            Console.WriteLine("   • Type the command starting with '/' at any input prompt");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n🌟 TIPS 🌟");
            Console.ResetColor();
            Console.WriteLine("• The faster you answer correctly, the more points you earn");
            Console.WriteLine("• Study the different categories to improve your knowledge");
            Console.WriteLine("• Create sessions with friends and compete for high scores");
        }

       
public static void PrintGalaxyHeader()
{
    // Clear console and set up
    Console.Clear();
    
    // Animated stars effect
    AnimateStars(50, 3000);
    
    // Main title with gradient effect
    string[] galaxyTitle = new string[]
    {
        @"   ___________________________________________________________",
        @"  /    _____       _             _   _        _               \",
        @" |    / ____|     | |           | | (_)      | |               |",
        @" |   | |  __  __ _| | __ ___  __| |_ _  ___  | |__             |",
        @" |   | | |_ |/ _` | |/ _` \ \/ /| __| |/ __| | '_ \            |",
        @" |   | |__| | (_| | | (_| |>  < | |_| | (__  | | | |           |",
        @" |    \_____|\__,_|_|\__,_/_/\_\ \__|_|\___| |_| |_|           |",
        @"  \___________________________________________________________/",
    };

    // Display main title with color gradient
    DisplayColorGradient(galaxyTitle, ConsoleColor.DarkMagenta, ConsoleColor.Cyan);
    
    // Subtitle with pulsing effect
    string[] subtitle = new string[]
    {
        @"     ☄️  EXPLORE THE UNIVERSE - DISCOVER NEW WORLDS  ☄️"
    };
    
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine();
    foreach (string line in subtitle)
    {
        Console.WriteLine(line);
    }
    
    // Elegant separator
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine("\n⭐ ════════════════════════════════════════════════════ ⭐");
    Console.ResetColor();
    Console.WriteLine();
}

// Displays text with a color gradient effect
private static void DisplayColorGradient(string[] text, ConsoleColor startColor, ConsoleColor endColor)
{
    // Preserve original colors
    ConsoleColor originalFg = Console.ForegroundColor;
    
    // Define colors for gradient (can be expanded)
    ConsoleColor[] gradientColors = new ConsoleColor[]
    {
        startColor,
        ConsoleColor.Magenta,
        ConsoleColor.Blue,
        ConsoleColor.Cyan,
        endColor
    };
    
    // Display each line with appropriate color
    for (int i = 0; i < text.Length; i++)
    {
        // Calculate which color to use based on position
        int colorIndex = (int)Math.Floor((double)i / text.Length * gradientColors.Length);
        if (colorIndex >= gradientColors.Length) colorIndex = gradientColors.Length - 1;
        
        Console.ForegroundColor = gradientColors[colorIndex];
        Console.WriteLine(text[i]);
    }
    
    // Reset color
    Console.ForegroundColor = originalFg;
}

// Creates animated stars effect
private static void AnimateStars(int numStars, int duration)
{
    Random rand = new Random();
    int consoleWidth = Console.WindowWidth;
    int consoleHeight = Console.WindowHeight;
    
    // Generate random star positions
    (int x, int y, char symbol)[] stars = new (int, int, char)[numStars];
    for (int i = 0; i < numStars; i++)
    {
        stars[i] = (
            rand.Next(consoleWidth), 
            rand.Next(consoleHeight), 
            rand.Next(2) == 0 ? '*' : '.'
        );
    }
    
    // Animate stars twinkling
    int frames = 5;
    for (int frame = 0; frame < frames; frame++)
    {
        Console.Clear();
        
        // Draw stars
        foreach (var star in stars)
        {
            try
            {
                Console.SetCursorPosition(star.x, star.y);
                Console.ForegroundColor = frame % 2 == 0 ? ConsoleColor.White : ConsoleColor.Gray;
                Console.Write(star.symbol);
            }
            catch (ArgumentOutOfRangeException)
            {
                // Skip if position is outside console
            }
        }
        
        // Pause between frames
        System.Threading.Thread.Sleep(duration / frames);
    }
    
    Console.Clear();
}
        public static void ShowHelp(Dictionary<string,string> COMMANDS)
        {
            Console.Clear();
            PrintGalaxyHeader();
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n📖 AVAILABLE COMMANDS");
            Console.ResetColor();
            
            Console.WriteLine($"\nUse {CMD_PREFIX}[command] to execute any of these commands:");
            
            foreach (var cmd in COMMANDS)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{CMD_PREFIX}{cmd.Key,-15}");
                Console.ResetColor();
                Console.WriteLine($" - {cmd.Value}");
            }
            
            Console.WriteLine("\nYou can use commands at any input prompt in the application.");
        }

        public static void ShowFeedback(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine("\n" + message);
            Console.ResetColor();
        }

        public static void Continue()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }

     static void DisplayFullQuestion(Question q, int current, int total, int secondsRemaining)
        {
            Console.Clear();
            UIService.PrintGalaxyHeader();
            
            // Placeholder for timer bar - will be updated separately
            Console.WriteLine($"⏱ Time: {secondsRemaining}s [" + new string(' ', Console.WindowWidth - 20) + "]");
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nQuestion {current}/{total}:");
            Console.ResetColor();
            
            // Make the question text much more visible with highlighting
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n{q.Text}\n");
            Console.ResetColor();
            Console.WriteLine(); // Extra spacing
            // Display answer options with clear formatting
            Console.WriteLine("Answer options:");
            for (int i = 0; i < q.Options.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{(char)('A' + i)}) ");
                Console.ResetColor();
                Console.WriteLine(q.Options[i]);
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\n👉 Press A, B, C or D to select your answer: ");
            Console.ResetColor();
            
            // Make answer input area very visible
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("     ");
            Console.ResetColor();
        }

         internal static void ShowFinalResults(Player player, Session session)
        {
            Console.Clear();
            UIService.PrintGalaxyHeader();

            SessionScore playerScore = SessionService.GetPlayerScore(player.Id, session.Id);
            int score = playerScore != null ? playerScore.Score : 0;
            int timeBonus = playerScore != null ? playerScore.TimeRemaining : 0;

            int totalQuestions = SessionService.GetSessionQuestionsCount(session.Id);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n🌟 Final Score: {score}/{totalQuestions} correct answers");
            Console.WriteLine($"⏱ Time Bonus: {timeBonus} points");
            Console.WriteLine($"🏆 Total Score: {score + timeBonus} points\n");

            // Get leaderboard
            Console.WriteLine("🏆 Leaderboard:");
            var leaderboard = SessionService.GetSessionLeaderboard(session.Id, AuthenticationService.GetAllPlayers());
            
            for (int i = 0; i < leaderboard.Count; i++)
            {
                var entry = leaderboard[i];
                Console.WriteLine($"{i+1}. {entry.Name}: {entry.Score} correct + {entry.TimeBonus} time bonus = {entry.Total} points");
            }

            Console.ResetColor();
        }

        internal static void UpdateTimerOnly(int row, int secondsRemaining, int totalSeconds)
        {
            int originalRow = Console.CursorTop;
            int originalCol = Console.CursorLeft;
            
            // Move to timer position
            Console.SetCursorPosition(0, row);
            
            // Calculate bar width
            int barWidth = Console.WindowWidth - 20;
            int filledWidth = (int)((double)secondsRemaining / totalSeconds * barWidth);
            
            // Update timer with appropriate color
            Console.ForegroundColor = secondsRemaining > 10 ? ConsoleColor.Green : 
                                    secondsRemaining > 5 ? ConsoleColor.Yellow : ConsoleColor.Red;
            Console.Write($"⏱ Time: {secondsRemaining}s [");
            Console.Write(new string('■', filledWidth));
            Console.Write(new string('□', barWidth - filledWidth));
            Console.Write("]");
            Console.ResetColor();
            
            // Move cursor back to original position
            Console.SetCursorPosition(originalCol, originalRow);
        }




    }
}
