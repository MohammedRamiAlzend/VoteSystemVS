namespace Domain.Entities;

public class VoteSession : Entity
{
  public string TopicTitle { get; set; }
  public string? Description { get; set; }
  public DateTime StartedAt { get; set; }
  public DateTime EndedAt { get; set; } 
  public bool IsActive { get; set; }
}