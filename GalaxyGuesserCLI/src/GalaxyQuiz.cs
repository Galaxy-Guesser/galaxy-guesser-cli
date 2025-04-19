using System.Text;
using ConsoleApp1.Models;
using ConsoleApp1.Services;
using ConsoleApp1.Utilities;
using Spectre.Console;
 
namespace ConsoleApp1
{
    class GalaxyQuiz
    {
        public static async Task Start()
{
    Console.OutputEncoding = Encoding.UTF8;
    Console.Title = "🌌 Galaxy Quiz";
    Console.CursorVisible = false;

    try
    {
        var httpClient = new HttpClient();
        var nasaService = new NasaService(httpClient);

        UIService.PrintGalaxyHeader();
        var authService = new AuthenticationService();
        var jwt = await authService.AuthenticateWithGoogle();

        var player = await authService.AuthOrRegisterWithBackend();

        var fact = await nasaService.GetSpaceFactAsync();

        UIService.DisplaySpaceFact(fact.explanation);

        await MainMenuLoop(player);
    }
    catch (Exception exception)
    {
        ErrorHandler.HandleError(exception);
    }
}

        static async Task MainMenuLoop(Player player)
        {
            bool exitRequested = false;

            while (!exitRequested)
            {
                AnsiConsole.Clear();

                UIService.PrintGalaxyHeader();
                AnsiConsole.MarkupLine($"\n👋 Welcome, [bold]{player.userName}[/]!\n");

                var menuActions = new Dictionary<string, Func<Task>>
                {
                    ["Create new quiz session"] = async () => {
                        var (category, questionCount, startTime, sessionDuration) = await SessionUIService.PromptSessionDetails();
                        AnsiConsole.MarkupLine($"Category: [cyan]{category}[/], Question Count: [cyan]{questionCount}[/], Start Time: [cyan]{startTime}[/]");

                        string sessionCode = await SessionService.CreateSessionAsync(category, questionCount, startTime, sessionDuration);

                        if (!string.IsNullOrEmpty(sessionCode))
                            AnsiConsole.MarkupLine($"✅ Session created! Code: [bold yellow]{sessionCode}[/]");
                        else
                            AnsiConsole.MarkupLine("[red]❌ Failed to create session.[/]");
                    },

                    ["Join existing session"] = async () => {
                        var activeSessions = await SessionViewService.GetActiveSessions();

                        if (activeSessions.Count == 0)
                        {
                            AnsiConsole.MarkupLine("[red]No active sessions available.[/]");
                            return;
                        }

                        var choices = activeSessions.Select(s => $"{s.sessionCode} - {s.category}").ToList();
                        choices.Add("↩️ Back to Main Menu");

                        var selectedOption = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("Select a session to join")
                            .PageSize(10)
                            .AddChoices(choices));

                        if (selectedOption == "↩️ Back to Main Menu")
                            return;

                        string sessionCode = selectedOption.Split(" - ")[0];
                        await SessionService.JoinSessionAsync(sessionCode);
                    },

                    ["View active sessions"] = async () => {
                        var sessions = await SessionViewService.GetActiveSessions();
                        await UIService.DisplayActiveSessionsAsync(sessions);
                    },

                    ["Leaderboards"] = async () => {
                        await ShowLeaderboardMenu(player);
                    },

                    ["My profile"] = async () => {
                        await ShowProfileMenu(player);
                    },

                    ["How to play"] = () => {
                        UIService.ShowHowToPlay();
                        return Task.CompletedTask;
                    },

                    ["Exit"] = () => {
                        if (AnsiConsole.Confirm("Are you sure you want to exit?"))
                        {
                            exitRequested = true;
                            AnsiConsole.MarkupLine("\n[green]👋 Thanks for playing Galaxy Quiz! See you among the stars![/]");
                            Thread.Sleep(1000);
                        }
                        return Task.CompletedTask;
                    }
                };

                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("MAIN MENU")
                    .PageSize(10)
                    .AddChoices(menuActions.Keys));

                await menuActions[selection]();

                if (!exitRequested)
                    UIService.Continue();
            }
        }

        private static async Task ShowProfileMenu(Player player)
        {
            bool backToMainMenu = false;

            while (!backToMainMenu)
            {
                AnsiConsole.Clear();
                UIService.PrintGalaxyHeader();
                AnsiConsole.MarkupLine($"\n[bold cyan]PROFILE MENU[/] - {player.userName}\n");

                var profileOptions = new Dictionary<string, Func<Task>>
                {
                    ["View Profile"] = () => {
                        CommandService.ProcessCommand("myprofile", player);
                        return Task.CompletedTask;
                    },

                    ["View Total Stats"] = async () => {
                        CommandService.ProcessCommand("totalstats", player);
                    },

                    ["Change Username"] = async () => {
                        CommandService.ProcessCommand("editusername", player);
                    },

                    ["↩️ Back to Main Menu"] = () => {
                        backToMainMenu = true;
                        return Task.CompletedTask;
                    }
                };

                var profileSelection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Select an option")
                    .PageSize(10)
                    .AddChoices(profileOptions.Keys));

                await profileOptions[profileSelection]();

                if (!backToMainMenu)
                    UIService.Continue();
            }
        }

        private static async Task ShowLeaderboardMenu(Player player)
        {
            bool backToMainMenu = false;

            while (!backToMainMenu)
            {
                AnsiConsole.Clear();
                UIService.PrintGalaxyHeader();
                AnsiConsole.MarkupLine("\n[bold cyan]LEADERBOARDS[/]\n");

                var leaderboardOptions = new Dictionary<string, Func<Task>>
                {
                    ["Global Leaderboard"] = async () => {
                        await UIService.DisplayGlobalLeaderboard();
                    },
                    ["Session Leaderboard"] = async () => {
                      await UIService.DisplaySessionLeaderboard();
                    },
                    ["↩️ Back to Main Menu"] = () => {
                        backToMainMenu = true;
                        return Task.CompletedTask;
                    }
                };

                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Select leaderboard type")
                    .PageSize(10)
                    .AddChoices(leaderboardOptions.Keys));

                await leaderboardOptions[selection]();

                if (!backToMainMenu)
                    UIService.Continue();
            }
        }
    }
}