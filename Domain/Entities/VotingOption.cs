
namespace Domain.Entities;
public class VotingOption
{
    /// <summary>
    /// Gets or sets the ID of the voting item this option belongs to.
    /// </summary>
    public Guid VotingItemId { get; set; }
    /// <summary>
    /// Gets or sets the text of the voting option.
    /// </summary>
    public string OptionText { get; set; }
    /// <summary>
    /// Gets or sets the display order of the option.
    /// </summary>
    public int DisplayOrder { get; set; } = 0;
    /// <summary>
    /// Gets or sets a value indicating whether this is a write-in option.
    /// </summary>
    public bool IsWriteIn { get; set; } = false;
    /// <summary>
    /// Gets or sets a value indicating whether this option is marked as deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    // العلاقات
    /// <summary>
    /// Gets or sets the voting item this option belongs to.
    /// </summary>
    public VotingItem VotingItem { get; set; }
    /// <summary>
    /// Gets or sets the collection of votes cast for this option.
    /// </summary>
    public ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
