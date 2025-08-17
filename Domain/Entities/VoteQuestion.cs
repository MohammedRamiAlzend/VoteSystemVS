using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class VoteQuestion : Entity
{
  public string Title { get; set; }

  public string? Description { get; set; }

  public DateTime StartedAt { get; set; }
  public DateTime EndedAt { get; set; }
  public int VoteSessionId{ get; set; }
  public VoteSession VoteSession{ get; set; }
  public ICollection<VoteQuestionOption> Options { get; set; } = [];
}
