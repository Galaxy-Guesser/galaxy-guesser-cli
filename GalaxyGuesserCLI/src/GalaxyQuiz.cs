using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using ConsoleApp1.Models;
using ConsoleApp1.Services;
using ConsoleApp1.Data;
using ConsoleApp1.Utilities;

namespace ConsoleApp1
{
    class GalaxyQuiz
    {
        public static void Start()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "🌌 Galaxy Quiz";
            Console.CursorVisible = false;

            try
            {
                UIService.PrintGalaxyHeader();
                Player player = AuthenticatePlayer();
                MainMenuLoop(player);
            }
            catch (Exception exception)
            {
                ErrorHandler.HandleError(exception);
            }
        }

        static void MainMenuLoop(Player player)
        {
            bool exitRequested = false;
            
            while (!exitRequested)
            {
                UIService.PrintGalaxyHeader();
                Console.WriteLine($"\n👋 Welcome, {player.Name}!");
                Console.WriteLine("\nMAIN MENU");
                Console.WriteLine("1. Create new quiz session");
                Console.WriteLine("2. Join existing session");
                Console.WriteLine("3. View categories");
                Console.WriteLine("4. View leaderboard");
                Console.WriteLine("5. My profile");
                Console.WriteLine("6. How to play");
                Console.WriteLine("7. Exit");
                Console.WriteLine("\nType a command (e.g., '/help') or select an option (1-7):");
                
                Console.Write("\n👉 ");
                Console.CursorVisible = true;
                string input = Console.ReadLine().Trim();
                Console.CursorVisible = false;
                
                // Check if input is a command
                if (CommandService.IsCommand(input))
                {
                    string cmd = CommandService.ExtractCommandName(input);
                    CommandService.ProcessCommand(cmd, player);
                    UIService.Continue();
                }
                else if (int.TryParse(input, out int option))
                {
                    switch (option)
                    {
                        case 1:
                            Session newSession = CreateSession(player);
                            if (newSession != null)
                            {
                                PlayQuizSession(player, newSession);
                                UIService.ShowFinalResults(player, newSession);
                            }
                            UIService.Continue();
                            break;
                        case 2:
                            Session joinSession = JoinSession(player);
                            if (joinSession != null)
                            {
                                PlayQuizSession(player, joinSession);
                                UIService.ShowFinalResults(player, joinSession);
                            }
                            UIService.Continue();
                            break;
                        case 3:
                            CommandService.ProcessCommand("categories", player);
                            UIService.Continue();
                            break;
                        case 4:
                            CommandService.ProcessCommand("leaderboard", player);
                            UIService.Continue();
                            break;
                        case 5:
                            CommandService.ProcessCommand("myprofile", player);
                            UIService.Continue();
                            break;
                        case 6:
                            UIService.ShowHowToPlay();
                            UIService.Continue();
                            break;
                        case 7:
                            exitRequested = true;
                            Console.WriteLine("\n👋 Thanks for playing Galaxy Quiz! See you among the stars!");
                            Thread.Sleep(2000);
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            UIService.Continue();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number or command.");
                    UIService.Continue();
                }
            }
        }

        static Player AuthenticatePlayer()
        {
            while (true)
            {
                Console.WriteLine("\n1. Login\n2. Register new account");
                Console.Write("\n👉 Enter your choice (1 or 2): ");
                Console.CursorVisible = true;
                
                ConsoleKey choice = Console.ReadKey(true).Key;
                Console.CursorVisible = false;
                
                if (choice == ConsoleKey.D1)
                {
                    Console.WriteLine("1");
                    // Login
                    Console.Write("\nUsername: ");
                    Console.CursorVisible = true;
                    string username = Console.ReadLine();
                    Console.CursorVisible = false;
                    
                    Console.Write("Password: ");
                    Console.CursorVisible = true;
                    string password = AuthenticationService.ReadPassword();
                    Console.CursorVisible = false;

                    Player player = AuthenticationService.Login(username, password);
                    
                    if (player != null)
                    {
                        Console.WriteLine($"\n👋 Welcome back, {player.Name}!");
                        Thread.Sleep(1500);
                        return player;
                    }
                    else if (SampleData.UserCredentials.ContainsKey(username))
                    {
                        // Valid credentials but no profile
                        Console.Write("\nEnter your display name: ");
                        Console.CursorVisible = true;
                        string name = Console.ReadLine();
                        Console.CursorVisible = false;

                        player = AuthenticationService.CreateProfileForExistingCredentials(username, name);
                        Console.WriteLine($"\n👋 Welcome, {player.Name}!");
                        Thread.Sleep(1500);
                        return player;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n❌ Invalid username or password. Try again.");
                        Console.ResetColor();
                    }
                }
                else if (choice == ConsoleKey.D2)
                {
                    Console.WriteLine("2");
                    // Register
                    Console.Write("\nCreate username: ");
                    Console.CursorVisible = true;
                    string username = Console.ReadLine();
                    Console.CursorVisible = false;

                    if (SampleData.UserCredentials.ContainsKey(username))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Username already exists. Try another one.");
                        Console.ResetColor();
                        continue;
                    }

                    Console.Write("Create password: ");
                    Console.CursorVisible = true;
                    string password = AuthenticationService.ReadPassword();
                    Console.CursorVisible = false;
                    
                    Console.Write("\nEnter your display name: ");
                    Console.CursorVisible = true;
                    string name = Console.ReadLine();
                    Console.CursorVisible = false;

                    Player player = AuthenticationService.Register(username, password, name);
                    if (player != null)
                    {
                        Console.WriteLine($"\n✅ Registration successful! Welcome, {player.Name}!");
                        Thread.Sleep(1500);
                        return player;
                    }
                }
            }
        }

        static Session CreateSession(Player player)
        {
            Console.WriteLine("\nAvailable categories:");
            for (int i = 0; i < SampleData.Categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {SampleData.Categories[i].Name}");
            }

            Console.Write("\n👉 Select category (1-" + SampleData.Categories.Count + "): ");
            Console.CursorVisible = true;
            int catChoice;
            while (!int.TryParse(Console.ReadLine(), out catChoice) || catChoice < 1 || catChoice > SampleData.Categories.Count)
            {
                Console.Write("Invalid choice. Please select again: ");
            }

            Console.Write("\n👉 How many questions do you want (1-10)? ");
            int questionCount;
            while (!int.TryParse(Console.ReadLine(), out questionCount) || questionCount < 1 || questionCount > 10)
            {
                Console.Write("Please enter a number between 1 and 10: ");
            }

            Console.Write("\n👉 Time per question in seconds (10-60): ");
            int questionDuration;
            while (!int.TryParse(Console.ReadLine(), out questionDuration) || questionDuration < 10 || questionDuration > 60)
            {
                Console.Write("Please enter a number between 10 and 60: ");
            }
            Console.CursorVisible = false;

            Session session = SessionService.CreateSession(
                player, 
                SampleData.Categories[catChoice - 1].Id, 
                questionCount, 
                questionDuration
            );

            Console.WriteLine($"\n🎮 Session created! Code: {session.Code}");
            Thread.Sleep(2000);

            return session;
        }

        static Session JoinSession(Player player)
        {
            Console.Write("\n👉 Enter session code: ");
            Console.CursorVisible = true;
            string sessionCode = Console.ReadLine().ToUpper();
            Console.CursorVisible = false;

            Session session = SessionService.JoinSession(player, sessionCode);

            if (session != null)
            {
                Console.WriteLine("\n🚀 Joining session...");
                Thread.Sleep(1500);
                return session;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Session not found. Please check the code and try again.");
                Console.ResetColor();
                return null;
            }
        }

        static void PlayQuizSession(Player player, Session session)
        {
            List<Question> sessionQuestions = SessionService.GetSessionQuestions(session.Id);
            int score = 0;
            int totalTimeRemaining = 0;

            for (int i = 0; i < sessionQuestions.Count; i++)
            {
                bool answered = false;
                int selectedOption = -1;
                int timeRemaining = session.QuestionDuration;

                // First render the full question and options (once only)
                DisplayFullQuestion(sessionQuestions[i], i+1, sessionQuestions.Count, timeRemaining);

                // Display timer
                int timerRow = Console.CursorTop - sessionQuestions[i].Options.Length - 4;
                
                // Start timer display thread
                CancellationTokenSource cts = new CancellationTokenSource();
                Task timerTask = Task.Run(() => 
                {
                    try
                    {
                        while (timeRemaining > 0 && !cts.Token.IsCancellationRequested)
                        {
                            UIService.UpdateTimerOnly(timerRow, timeRemaining, session.QuestionDuration);
                            Thread.Sleep(1000);
                            timeRemaining--;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // Task was canceled, do nothing
                    }
                }, cts.Token);

                Task<(bool answered, int selectedOption)> answerTask = SessionService.WaitForAnswerWithTimeout(
                    sessionQuestions[i], session.QuestionDuration);
                answerTask.Wait();
                
                cts.Cancel();
                
                answered = answerTask.Result.answered;
                selectedOption = answerTask.Result.selectedOption;
                
                int inputRow = Console.CursorTop;
                Console.SetCursorPosition(0, inputRow);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, inputRow);

                if (answered)
                {
                    Console.Write($"👉 Your answer: {(char)('A' + selectedOption)}");

                    if (selectedOption == sessionQuestions[i].CorrectAnswerIndex)
                    {
                        score++;
                        totalTimeRemaining += timeRemaining; // Award bonus points for quick answers
                        UIService.ShowFeedback($"✅ Correct! +{timeRemaining} time bonus!", ConsoleColor.Green);
                    }
                    else
                    {
                        UIService.ShowFeedback($"❌ Wrong! Correct was {(char)('A' + sessionQuestions[i].CorrectAnswerIndex)}", 
                                   ConsoleColor.Red);
                    }
                }
                else
                {
                    UIService.ShowFeedback($"⏰ Time's up! Correct was {(char)('A' + sessionQuestions[i].CorrectAnswerIndex)}", 
                               ConsoleColor.Yellow);
                }

                Thread.Sleep(1500);
            }

            // Save score
            SessionService.SaveScore(player.Id, session.Id, score, totalTimeRemaining);
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
    }
}