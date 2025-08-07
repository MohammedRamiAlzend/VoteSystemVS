namespace Domain.Entities;

public class SystemLog : Entity
{
  public string Action { get; set; }
  public string PerformedBy { get; set; }
  public DateTime TimeStamp{ get; set; }
}