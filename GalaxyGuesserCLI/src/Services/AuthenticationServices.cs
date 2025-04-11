using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleApp1.Models;
using ConsoleApp1.Data;

namespace ConsoleApp1.Services
{
    public class AuthenticationService
    {
        // In-memory data storage (could be moved to a repository pattern in future)
        private static List<Player> players = new List<Player>();
        private static Dictionary<string, string> userCredentials = SampleData.UserCredentials;

        internal static Player Login(string username, string password)
        {
            // Verify credentials
            if (userCredentials.TryGetValue(username, out string storedPassword) && password == storedPassword)
            {
                // Find existing player
                Player existingPlayer = players.FirstOrDefault(p => p.Username == username);
                if (existingPlayer != null)
                {
                    return existingPlayer;
                }
                else
                {
                    // Player credentials exist, but no profile. Need to create profile.
                    return null;
                }
            }
            
            // Login failed
            return null;
        }

        internal static Player Register(string username, string password, string displayName)
        {
            // Check if username exists
            if (userCredentials.ContainsKey(username))
            {
                return null; // Username already exists
            }

            // Save credentials
            userCredentials.Add(username, password);

            // Create player
            int playerId = players.Count > 0 ? players.Max(p => p.Id) + 1 : 1;
            Guid playerGuid = Guid.NewGuid();

            Player player = new Player(playerId, playerGuid, username, displayName);
            players.Add(player);

            return player;
        }

        internal static Player CreateProfileForExistingCredentials(string username, string displayName)
        {
            int playerId = players.Count > 0 ? players.Max(p => p.Id) + 1 : 1;
            Guid playerGuid = Guid.NewGuid();

            Player player = new Player(playerId, playerGuid, username, displayName);
            players.Add(player);

            return player;
        }

        internal static Player GetPlayerById(int id)
        {
            return players.FirstOrDefault(p => p.Id == id);
        }

        internal static List<Player> GetAllPlayers()
        {
            return players;
        }

        public static string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                {
                    password.Append(key.KeyChar);
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password.ToString();
        }
    }
}