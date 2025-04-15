using GalaxyGuesserApi.Models;

public class Option
{
  public int Id { get; set; }
  public string Text { get; set; }

  public int QuestionId { get; set; }
  public Question Question { get; set; }

  public bool IsCorrect { get; set; }
}