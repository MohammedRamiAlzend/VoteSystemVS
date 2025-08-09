namespace Application.Features.VoteQuestionGroup.Common;

public class VoteQuestionDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
    public int VoteSessionId { get; set; }
    public List<VoteQuestionOptionDto> Options { get; set; } = new List<VoteQuestionOptionDto>();
}

public class VoteQuestionOptionDto
{
    public int Id { get; set; }
    public string Title { get; set; }
}