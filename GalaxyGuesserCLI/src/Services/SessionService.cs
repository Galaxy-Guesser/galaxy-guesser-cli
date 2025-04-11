using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using ConsoleApp1.Data;

namespace ConsoleApp1.Services
{
    public class SessionService
    {
        // In-memory data storage (could be moved to a repository pattern in future)
        private static List<Session> sessions = new List<Session>();
        private static List<SessionPlayer> sessionPlayers = new List<SessionPlayer>();
        private static List<SessionQuestion> sessionQuestions = new List<SessionQuestion>();
        private static List<SessionScore> sessionScores = new List<SessionScore>();

        

        public static string GenerateSessionCode()
        {
            // Generate a unique 6-character code
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            Random random = new Random();
            string code;
            
            do
            {
                code = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            } while (sessions.Any(s => s.Code == code));
            
            return code;
        }

        internal static Session CreateSession(Player player, int categoryId, int questionCount, int questionDuration)
        {
            string sessionCode = GenerateSessionCode();
            int sessionId = sessions.Count > 0 ? sessions.Max(s => s.Id) + 1 : 1;
            
            Session session = new Session(sessionId, sessionCode, categoryId, questionDuration, questionCount);
            sessions.Add(session);
            
            // Add questions to session
            AddQuestionsToSession(session.Id, categoryId, questionCount);
            
            // Add player to session
            AddPlayerToSession(player.Id, session.Id);
            
            return session;
        }

        internal static Session JoinSession(Player player, string sessionCode)
        {
            Session session = sessions.FirstOrDefault(s => s.Code == sessionCode.ToUpper());
            
            if (session != null)
            {
                // Add player to session
                AddPlayerToSession(player.Id, session.Id);
                return session;
            }
            
            return null; // Session not found
        }

        public static void AddPlayerToSession(int playerId, int sessionId)
        {
            // Check if player already in session
            if (!sessionPlayers.Any(sp => sp.PlayerId == playerId && sp.SessionId == sessionId))
            {
                sessionPlayers.Add(new SessionPlayer(sessionId, playerId));
            }
        }

        public static void AddQuestionsToSession(int sessionId, int categoryId, int questionCount)
        {
            // Get questions for this category and randomize them
            var categoryQuestions = SampleData.Questions
                .Where(q => q.CategoryId == categoryId)
                .OrderBy(q => Guid.NewGuid()) // Random order
                .Take(questionCount)  // Only take requested number of questions
                .ToList();
            
            int id = sessionQuestions.Count > 0 ? sessionQuestions.Max(sq => sq.Id) + 1 : 1;
            foreach (var question in categoryQuestions)
            {
                sessionQuestions.Add(new SessionQuestion(id++, sessionId, question.Id));
            }
        }

        internal static List<Question> GetSessionQuestions(int sessionId)
        {
            return sessionQuestions
                .Where(sq => sq.SessionId == sessionId)
                .Join(
                    SampleData.Questions, 
                    sq => sq.QuestionId, 
                    q => q.Id, 
                    (sq, q) => q
                )
                .ToList();
        }

        public static void SaveScore(int playerId, int sessionId, int score, int timeRemaining = 0)
        {
            sessionScores.Add(new SessionScore(playerId, sessionId, score, timeRemaining));
        }

        public static SessionScore GetPlayerScore(int playerId, int sessionId)
        {
            return sessionScores.FirstOrDefault(s => s.PlayerId == playerId && s.SessionId == sessionId);
        }

        internal static List<dynamic> GetSessionLeaderboard(int sessionId, List<Player> players)
        {
            return sessionScores
                .Where(s => s.SessionId == sessionId)
                .OrderByDescending(s => s.Score + s.TimeRemaining)
                .Select(s => new { 
                    Name = players.First(p => p.Id == s.PlayerId).Name, 
                    Score = s.Score,
                    TimeBonus = s.TimeRemaining,
                    Total = s.Score + s.TimeRemaining
                })
                .ToList<dynamic>();
        }

        internal static async Task<(bool answered, int selectedOption)> WaitForAnswerWithTimeout(Question question, int timeoutSeconds)
        {
            var answerTask = Task.Run(() =>
            {
                ConsoleKeyInfo key;
                int selectedOption;
                do
                {
                    key = Console.ReadKey(true);
                    selectedOption = char.ToUpper(key.KeyChar) - 'A';
                } while (selectedOption < 0 || selectedOption >= question.Options.Length);

                return selectedOption;
            });

            var delayTask = Task.Delay(timeoutSeconds * 1000);
            var completedTask = await Task.WhenAny(answerTask, delayTask);

            if (completedTask == answerTask)
            {
                return (true, await answerTask);
            }
            else
            {
                return (false, -1);
            }
        }

        

        public static int GetSessionQuestionsCount(int sessionId)
        {
            return sessionQuestions.Count(sq => sq.SessionId == sessionId);
        }
    }
}