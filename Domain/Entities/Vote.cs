namespace Domain.Entities;

public class Vote : Entity
{
  public DateTime VotedAt { get; set; }
  public int UserId { get; set; }
  public User User { get; set; }
  public int VoteQuestionOptionId { get; set; }
  public VoteQuestionOption VoteQuestionOption { get; set; }
}
