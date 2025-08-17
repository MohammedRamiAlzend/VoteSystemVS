namespace Application.Features.VoteGroup.Common;

public class VoteDTO
{
    public int Id { get; set; }
    public DateTime VotedAt { get; set; }
    public int UserId { get; set; }
    public int VoteQuestionOptionId { get; set; }
}