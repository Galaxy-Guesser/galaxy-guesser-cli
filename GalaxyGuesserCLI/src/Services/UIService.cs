using Spectre.Console;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using ConsoleApp1.Models;
using System.Text;
using GalaxyGuesserCLI.Services;

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
    AnsiConsole.Clear();
    PrintGalaxyHeader();

    AnsiConsole.Write(
        new FigletText("HOW TO PLAY")
         
            .Color(Color.Cyan1));
    
    AnsiConsole.Write(
        new FigletText("GALAXY QUIZ")
         
            .Color(Color.MediumPurple2));

    // Create a grid layout for the instructions
    var grid = new Grid()
        .AddColumn(new GridColumn().PadRight(1))
        .AddColumn(new GridColumn().PadLeft(1))
        .AddRow();

    // Left column - instruction steps
    var leftPanel = new Panel(
        new Rows(
            new Markup("[bold cyan]1️⃣ CREATE OR JOIN A SESSION[/]"),
            new Text("• Create a new quiz session by selecting a category,"),
            new Text("  question count, and time limit"),
            new Text("• Join existing sessions with a session code"),
            new Markup("\n[bold cyan]2️⃣ ANSWER THE QUESTIONS[/]"),
            new Text("• Multiple-choice answers (A, B, C, D)"),
            new Text("• Select answers quickly for bonus points"),
            new Text("• Correct answers earn base points"),
            new Markup("\n[bold cyan]3️⃣ SCORING SYSTEM[/]"),
            new Text("• 1 point for each correct answer"),
            new Text("• 1 bonus point per second remaining"),
            new Text("• Final score = correct + time bonus")
        ))
    {
        Border = BoxBorder.Rounded,
        BorderStyle = new Style(Color.Cyan1),
        Padding = new Padding(1, 1, 1, 1)
    };

    // Right column - more instructions
    var rightPanel = new Panel(
        new Rows(
            new Markup("[bold cyan]4️⃣ LEADERBOARDS[/]"),
            new Text("• Compare scores with other players"),
            new Text("• View your statistics and history"),
            new Markup("\n[bold cyan]5️⃣ COMMANDS[/]"),
            new Text("• /help - Show available commands"),
            new Text("• /categories - List categories"),
            new Text("• /sessions - Active sessions"),
            new Markup("\n[bold yellow]🌟 TIPS 🌟[/]"),
            new Text("• Faster answers = more points"),
            new Text("• Study categories to improve"),
            new Text("• Compete with friends")
        ))
    {
        Border = BoxBorder.Rounded,
        BorderStyle = new Style(Color.MediumPurple2),
        Padding = new Padding(1, 1, 1, 1)
    };

    // Add panels to grid
    grid.AddRow(leftPanel, rightPanel);

    // Render the grid
    AnsiConsole.Write(grid);

    // Footer with decorative elements
    AnsiConsole.WriteLine();
    AnsiConsole.Write(new Rule("[yellow]Press any key to continue...[/]")
        .RuleStyle("grey dim"));
}

       
public static void PrintGalaxyHeader()
{
    Console.Clear();

    
    string[] galaxyTitle = new string[]
    {
        @"   ╔═══════════════════════════════════════════════════════════╗",
        @"   ║                                                           ║",
        @"   ║   ██████╗  █████╗ ██╗  ██╗ ██████╗  ██████╗ ████████╗    ║",
        @"   ║  ██╔════╝ ██╔══██╗██║  ██║██╔═══██╗██╔═══██╗╚══██╔══╝    ║",
        @"   ║  ██║  ███╗███████║███████║██║   ██║██║   ██║   ██║       ║",
        @"   ║  ██║   ██║██╔══██║██╔══██║██║   ██║██║   ██║   ██║       ║",
        @"   ║  ╚██████╔╝██║  ██║██║  ██║╚██████╔╝╚██████╔╝   ██║       ║",
        @"   ║   ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝  ╚═════╝    ╚═╝       ║",
        @"   ║                                                           ║",
        @"   ╚═══════════════════════════════════════════════════════════╝",
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

private static void DisplayColorGradient(string[] text, ConsoleColor startColor, ConsoleColor endColor)
{
    ConsoleColor originalFg = Console.ForegroundColor;
    
    ConsoleColor[] gradientColors = new ConsoleColor[]
    {
        startColor,
        ConsoleColor.Magenta,
        ConsoleColor.Blue,
        ConsoleColor.Cyan,
        endColor
    };
    
    for (int i = 0; i < text.Length; i++)
    {
        int colorIndex = (int)Math.Floor((double)i / text.Length * gradientColors.Length);
        if (colorIndex >= gradientColors.Length) colorIndex = gradientColors.Length - 1;

        Console.ForegroundColor = gradientColors[colorIndex];
        Console.WriteLine(text[i]);
    }
    
    Console.ForegroundColor = originalFg;
}

    public static void ShowHelp(Dictionary<string, string> COMMANDS)
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
      Console.WriteLine("\nReturn to main menu...");
      Console.ReadKey(true);
    }

    static void DisplayFullQuestion(Question q, int current, int total, int secondsRemaining)
    {
      Console.Clear();
      PrintGalaxyHeader();

      Console.WriteLine($"⏱ Time: {secondsRemaining}s [" + new string(' ', Console.WindowWidth - 20) + "]");

      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine($"\nQuestion {current}/{total}:");
      Console.ResetColor();

      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine($"\n{q.Text}\n");
      Console.ResetColor();
      Console.WriteLine(); // Extra spacing
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
      PrintGalaxyHeader();

      SessionScore playerScore = SessionService.GetPlayerScore(player.playerId, session.Id);
      int score = playerScore != null ? playerScore.Score : 0;
      int timeBonus = playerScore != null ? playerScore.TimeRemaining : 0;

      int totalQuestions = SessionService.GetSessionQuestionsCount(session.Id);

      Console.ForegroundColor = ConsoleColor.Magenta;
      Console.WriteLine($"\n🌟 Final Score: {score}/{totalQuestions} correct answers");
      Console.WriteLine($"⏱ Time Bonus: {timeBonus} points");
      Console.WriteLine($"🏆 Total Score: {score + timeBonus} points\n");

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
            
            Console.SetCursorPosition(0, row);
            
            int barWidth = Console.WindowWidth - 20;
            int filledWidth = (int)((double)secondsRemaining / totalSeconds * barWidth);
            
            Console.ForegroundColor = secondsRemaining > 10 ? ConsoleColor.Green : 
                                    secondsRemaining > 5 ? ConsoleColor.Yellow : ConsoleColor.Red;
            Console.Write($"⏱ Time: {secondsRemaining}s [");
            Console.Write(new string('■', filledWidth));
            Console.Write(new string('□', barWidth - filledWidth));
            Console.Write("]");
            Console.ResetColor();
            
            Console.SetCursorPosition(originalCol, originalRow);
        }

    public static async Task DisplayActiveSessionsAsync(List<SessionView> sessions)
    {
      AnsiConsole.MarkupLine("\n📡 [bold underline]Active Sessions[/]");

      if (sessions.Count == 0)
      {
        AnsiConsole.MarkupLine("[grey]No active sessions found.[/]");
        return;
      }

      var grid = new Grid();
      grid.AddColumn();
      grid.AddColumn();

      var panels = sessions.Select(session =>
      {
        var timeParts = session.endsIn.Split(new[] { 'm', 's' }, StringSplitOptions.RemoveEmptyEntries);
        int minutes = int.TryParse(timeParts[0], out var parsedMinutes) ? parsedMinutes : 0;

        var color = minutes < 5 ? "red" : "green";

        return new Panel(new Markup(
                  $"[bold]{session.category}[/]\n" +
                  $"[blue]Code:[/] {session.sessionCode}\n" +
                  $"[blue]Ends In:[/] [{color}]{minutes}m[/]"))
        {
          Border = BoxBorder.Double,
          Padding = new Padding(1, 0, 1, 0)
        };
      }).ToList();

      for (int i = 0; i < panels.Count; i += 2)
      {
        if (i + 1 < panels.Count)
          grid.AddRow(panels[i], panels[i + 1]);
        else
          grid.AddRow(panels[i]);
      }

      AnsiConsole.Write(grid);

      var sessionCode = AnsiConsole.Ask<string>("\n▶️ [bold yellow]Enter a session code to join or press Enter to cancel:[/]");

      if (!string.IsNullOrWhiteSpace(sessionCode))
      {
        var selectedSession = sessions.FirstOrDefault(s => s.sessionCode.Equals(sessionCode, StringComparison.OrdinalIgnoreCase));
        if (selectedSession != null)
        {
          await SessionService.JoinSessionAsync(sessionCode);
        }
        else
        {
          AnsiConsole.MarkupLine($"❌ [red]Invalid session code:[/] {sessionCode}");
        }
      }
      else
      {
        AnsiConsole.MarkupLine("[grey]No session joined.[/]");
      }
    }

    public static void DisplaySpaceFact(string fact)
    {
      Console.Clear();
      PrintGalaxyHeader();

      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine("🌟 Space Fact of the Day 🌟");
      Console.WriteLine(new string('─', Console.WindowWidth));
      Console.ResetColor();

      var words = fact.Split(' ');
      var line = new StringBuilder();
      foreach (var word in words)
      {
        if (line.Length + word.Length > Console.WindowWidth - 4)
        {
          Console.WriteLine($"  {line}");
          line.Clear();
        }
        line.Append(word + " ");
      }
      Console.WriteLine($"  {line}");

      Console.ForegroundColor = ConsoleColor.DarkGray;
      Console.WriteLine("\nCourtesy of NASA's Astronomy Picture of the Day");
      Console.ResetColor();

      Continue();
    }

        public static async Task DisplaySessionQuestionsAsync(List<SessionQuestionView> questions)
        {
            if (questions.Count == 0)
            {
                AnsiConsole.MarkupLine("[grey]No questions found for this session.[/]");
                return;
            }

            // Track the total score throughout the session
            int currentTotalScore = 0;

            foreach (var question in questions)
            {
                AnsiConsole.Clear();
                var questionPanel = new Panel(BuildQuestionMarkup(question))
                {
                    Border = BoxBorder.Rounded,
                    Padding = new Padding(1, 0, 1, 0),
                    Header = new PanelHeader($"[bold green]Question {question.questionId}[/]", Justify.Center)
                };

                AnsiConsole.Write(questionPanel);
                
                DisplayTotalScorePanel(currentTotalScore);

                var stopwatch = Stopwatch.StartNew();
                var timeLimit = 30; 
                var promptCts = new CancellationTokenSource();
                
                var timeRemaining = timeLimit;
                
                var timerTask = Task.Run(async () => 
                {
                    try 
                    {
                        int timerLeft = 25; 
                        int timerTop = Console.CursorTop;
                        
                        while (timeRemaining > 0 && !promptCts.Token.IsCancellationRequested)
                        {
                            int currentLeft = Console.CursorLeft;
                            int currentTop = Console.CursorTop;
                            
                            Console.SetCursorPosition(timerLeft, timerTop);
                            Console.ForegroundColor = timeRemaining <= 5 ? ConsoleColor.Red : ConsoleColor.Yellow;
                            Console.Write($"[{timeRemaining.ToString().PadLeft(2, '0')} seconds]");
                            Console.ResetColor();
                            
                            Console.SetCursorPosition(currentLeft, currentTop);
                            
                            await Task.Delay(1000, promptCts.Token);
                            timeRemaining--;
                        }
                        
                        if (timeRemaining <= 0 && !promptCts.Token.IsCancellationRequested)
                        {
                            promptCts.Cancel();
                        }
                    }
                    catch (OperationCanceledException) 
                    {
                        // Task was canceled, simply exit
                    }
                });

                try
                {
                    await Task.Delay(200);
                    
                    var prompt = new SelectionPrompt<string>()
                        .Title("\n\n[bold]Select your answer:[/]") 
                        .HighlightStyle("cyan");

                    foreach (var (option, index) in question.options.Select((o, i) => (o, i)))
                    {
                        prompt.AddChoice($"{index + 1}. {option.optionText}");
                    }

                    var selectedLabel = await AnsiConsole.PromptAsync(prompt, promptCts.Token);
                    stopwatch.Stop();
                    
                    promptCts.Cancel();
                    
                    try
                    {
                        await Task.WhenAll(timerTask);
                    }
                    catch { }

                    var selectedOption = question.options[int.Parse(selectedLabel.Split('.')[0]) - 1];
                    bool isCorrect = selectedOption.answerId == question.correctAnswerId;

                    // Find the correct answer to display
                    var correctOption = question.options.First(o => o.answerId == question.correctAnswerId);

                    if (isCorrect)
                    {
                        AnsiConsole.MarkupLine("\n[green]✔ Correct![/]");
                        int elapsedTime = (int)Math.Round(stopwatch.Elapsed.TotalSeconds);
                        var totalScore = await SessionScores.UpdatePlayerScores(question.sessionId,timeLimit - elapsedTime);
                        
                        currentTotalScore = totalScore.NewTotalScore;
                        
                        DisplayTotalScorePanel(currentTotalScore);
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"\n[red]✘ Incorrect.[/]");
                        AnsiConsole.MarkupLine($"[green]The correct answer was:[/] [bold green]{Array.FindIndex(question.options.ToArray(), o => o.answerId == question.correctAnswerId) + 1}. {correctOption.optionText}[/]");
                    }

                    AnsiConsole.MarkupLine($"[blue]Time taken: {stopwatch.Elapsed.TotalSeconds:F2} seconds[/]");
                }
                catch (TaskCanceledException)
                {
                    stopwatch.Stop();
                    
                    if (!promptCts.IsCancellationRequested)
                    {
                        promptCts.Cancel();
                    }
                    
                    try
                    {
                        await Task.WhenAll(timerTask);
                    }
                    catch { }

                    AnsiConsole.MarkupLine("\n[red]Time's up! Moving to next question...[/]");
                    AnsiConsole.MarkupLine($"[blue]Time taken: {stopwatch.Elapsed.TotalSeconds:F2} seconds[/]");
                }

                await Task.Delay(2000);
            }

            AnsiConsole.MarkupLine("[bold green]✅ All questions completed[/]");
        }

        private static void DisplayTotalScorePanel(int totalScore)
        {
            int originalTop = Console.CursorTop;
            int originalLeft = Console.CursorLeft;
            
            int leftPosition = 0;
            int bottomPosition = Console.WindowHeight - 6; 
            
            Console.SetCursorPosition(leftPosition, bottomPosition);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("┌─────────────────┐");
            Console.SetCursorPosition(leftPosition, bottomPosition + 1);
            Console.WriteLine("│  TOTAL SCORE    │");
            Console.SetCursorPosition(leftPosition, bottomPosition + 2);
            Console.WriteLine("└─────────────────┘");
            Console.SetCursorPosition(leftPosition, bottomPosition + 3);
            Console.WriteLine($"    {totalScore} points    ");
            Console.ResetColor();
            
            Console.SetCursorPosition(originalLeft, originalTop);
        }
        private static string BuildQuestionMarkup(SessionQuestionView question)
        {
            var markup = $"[bold underline]{question.questionText}[/]\n\n";
            for (int i = 0; i < question.options.Count; i++)
            {
                markup += $"[blue]{i + 1}.[/] {question.options[i].optionText}\n";
            }
            return markup;
        }
        public static async Task DisplayPlayerStats(Task<List<PlayerStatsDTO>> data)
    {
      try
      {
        UIService.PrintGalaxyHeader();
        AnsiConsole.MarkupLine($"\n[bold yellow]🚀 PLAYER STATS:[/]\n");

        var dataList = await data;

        // Extract key stats
        var highestScore = dataList.Max(x => x.sessionScore);
        var totalSessions = dataList.Select(x => x.sessionCode).Distinct().Count();

        // Main session stats table
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Blue)
            .AddColumn(new TableColumn("[bold]Session Code[/]").Centered())
            .AddColumn(new TableColumn("[bold]Category[/]").Centered())
            .AddColumn(new TableColumn("[bold]Score[/]").Centered())
            .AddColumn(new TableColumn("[bold]Ranking[/]").Centered());

        foreach (var entry in dataList)
        {
          table.AddRow(
              $"[bold]{entry.sessionCode}[/]",
              $"[bold]{entry.category}[/]",
              $"[bold]{entry.sessionScore}[/]",
              $"[bold]#{entry.ranking}[/]"
          );
        }

        AnsiConsole.Write(table);

        // Summary table
        var summaryTable = new Table()
            .Border(TableBorder.Square)
            .BorderColor(Color.Blue)
            .AddColumn(new TableColumn("[bold]Total Sessions[/]").Centered())
            .AddColumn(new TableColumn("[bold]Highest Score[/]").Centered());

        summaryTable.AddRow(
            $"[bold]{totalSessions}[/]",
            $"[bold]{highestScore}[/]"
        );

        AnsiConsole.WriteLine(); // Just a bit of spacing
        AnsiConsole.Write(summaryTable);
      }
      catch (Exception ex)
      {
        AnsiConsole.MarkupLine("[red]Error loading session leaderboard:[/]");
        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
      }
    }}
        public static async Task DisplayGlobalLeaderboard()
        {
             try
            {
                AnsiConsole.Status()
                    .Start("Loading global leaderboard...", ctx => 
                    {
                        ctx.Spinner(Spinner.Known.Star);
                        ctx.SpinnerStyle(Style.Parse("green"));
                    });

                var leaderboard = await LeaderboardService.GetGlobalLeaderboardAsync();

                AnsiConsole.Clear();
                UIService.PrintGalaxyHeader();
                AnsiConsole.MarkupLine("\n[bold yellow]🌌 GLOBAL LEADERBOARD[/]\n");

                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Purple)
                    .AddColumn(new TableColumn("[bold]Rank[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Player[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Total Score[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Sessions[/]").Centered());

                if (leaderboard.Count == 0)
                {
                    table.AddRow("[red]No data available[/]", "", "", "");
                }
                else
                {
                    foreach (var entry in leaderboard)
                    {
                        var sessionsList = entry.Sessions?.Any() == true 
                            ? string.Join("\n", entry.Sessions.Take(3)) 
                            : "None";
                        
                        if (entry.Sessions?.Count > 3)
                        {
                            sessionsList += $"\n...and {entry.Sessions.Count - 3} more";
                        }

                        table.AddRow(
                            $"[bold]#{entry.Rank}[/]",
                            Markup.Escape(entry.UserName),
                            entry.TotalScore.ToString(),
                            sessionsList
                        );
                    }
                }

                AnsiConsole.Write(table);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine("[red]Error loading leaderboard:[/] " + ex.Message);
            }
        }

        public static async Task DisplaySessionLeaderboard()
        {
            try
            {
                var sessionCode = AnsiConsole.Ask<string>("Enter session code:");
     
                AnsiConsole.Status()
                    .Start("Loading session leaderboard...", ctx => 
                    {
                        ctx.Spinner(Spinner.Known.Star);
                        ctx.SpinnerStyle(Style.Parse("blue"));
                    });

                var leaderboard = await LeaderboardService.GetSessionLeaderboardAsync(sessionCode);

                if (leaderboard != null)
                {
                    foreach (var entry in leaderboard.Take(3))
                    {
                        Console.WriteLine($"DEBUG: {entry.Rank}. {entry.UserName} - {entry.Score}");
                    }
                }

                AnsiConsole.Clear();
                UIService.PrintGalaxyHeader();
                AnsiConsole.MarkupLine($"\n[bold yellow]🚀 SESSION LEADERBOARD: {sessionCode}[/]\n");

                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Blue)
                    .AddColumn(new TableColumn("[bold]Rank[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Player[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Score[/]").Centered());

                if (leaderboard == null || leaderboard.Count == 0)
                {
                    table.AddRow("[red]No data available[/]", "Try a different session code", "");
                }
                else
                {
                    foreach (var entry in leaderboard)
                    {
                        table.AddRow(
                            $"[bold]#{entry.Rank}[/]",
                            Markup.Escape(entry.UserName ?? "Unknown"),
                            entry.Score.ToString()
                        );
                    }
                }

                AnsiConsole.Write(table);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine("[red]Error loading session leaderboard:[/]");
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
            }
        }
    }
}