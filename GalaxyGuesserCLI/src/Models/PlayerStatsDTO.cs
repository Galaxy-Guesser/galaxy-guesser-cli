namespace ConsoleApp1.Models
{
  public class PlayerStatsDTO
  {
    public string sessionCode { get; set; }
    public string category { get; set; }
    public int sessionScore { get; set; }
    public int ranking { get; set; }
    public int totalSessions { get; set; }
    public int highestScore { get; set; }
  }
}