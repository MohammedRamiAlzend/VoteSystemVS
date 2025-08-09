namespace Domain.Entities;

public class AttendanceUser : Entity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int VoteSessionId { get; set; }
    public VoteSession VoteSession { get; set; }
    public int? OTPCodeID { get; set; } // can be null because it will be added to vote session then he will login via otp code 
    public OTPCode OTPCode { get; set; }
    public Admin CreatedByAdmin { get; set; }
    public int CreatedByAdminId { get; set; }
}
