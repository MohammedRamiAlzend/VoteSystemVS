namespace Domain.Entities;

public class OTPCode : Entity
{
  public string Code { get; set; }
  public DateTime ExpiredAt { get; set; }
  public bool IsUsed { get; set; }
  public DateTime CreatedAt{ get; set; }
}