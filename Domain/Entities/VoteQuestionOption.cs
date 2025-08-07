namespace Domain.Entities;

public class VoteQuestionOption : Entity
{
  public string Title { get; set; }
  public int VoteQuestionId { get; set; }
  public VoteQuestion VoteQuestion { get; set; }
}
