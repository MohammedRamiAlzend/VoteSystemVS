namespace Application.Features.VoteSessionGroup.Common;

public class CreateVoteSessionDto
{
    public string TopicTitle { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
    public bool IsActive { get; set; }
}
