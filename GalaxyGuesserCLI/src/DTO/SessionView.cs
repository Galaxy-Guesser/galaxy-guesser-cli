namespace GalaxyGuesserCLI.DTO
{
    public class SessionView
    {
        public int SessionId { get; set; }
        public string sessionCode { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public List<string> playerUserNames { get; set; }
        public int playerCount { get; set; }
        public int questionCount { get; set; }
        public int? highestScore { get; set; }
        public string endsIn { get; set; } = string.Empty;
    }
}