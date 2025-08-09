namespace Domain.Entities;

public class VoteSession : Entity
{
    public string TopicTitle { get; set; }
    public string? Description { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
    public VoteSessionStatus VoteSessionStatus { get; set; } = VoteSessionStatus.Open;
    public ICollection<AttendanceUser> AttendanceUsers { get; set; } = [];
    public ICollection<VoteQuestion> Questions { get; set; } = [];

}

public enum VoteSessionStatus
{
    Open,
    Activated,
    Closed
}
