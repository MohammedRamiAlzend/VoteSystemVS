using Domain.Entities;

namespace Application.Features.VoteSessionGroup.Common;

public class VoteSessionDto
{
    public int Id { get; set; }
    public string TopicTitle { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
    public VoteSessionStatus VoteSessionStatus { get; set; }
}
