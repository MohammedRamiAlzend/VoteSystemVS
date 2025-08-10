namespace Domain.Entities;

public class OTPCode : Entity
{
    public string Code { get; set; }
    public DateTime ExpiredAt { get; set; }
    public bool IsUsed { get; set; }
    public DateTime CreatedAt { get; set; }
    public AttendanceUser AttendanceUser { get; set; }

    public bool IsOTPValid()
    {
        if (this.ExpiredAt < DateTime.UtcNow)
            return false;
        else
            return true;
    }
}
