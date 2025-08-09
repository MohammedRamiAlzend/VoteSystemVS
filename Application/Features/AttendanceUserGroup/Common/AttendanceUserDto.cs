namespace Application.Features.AttendanceUserGroup.Common;

public class AttendanceUserDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int VoteSessionId { get; set; }
    public int? OTPCodeID { get; set; }
    public int? CreatedByAdminId { get; set; }
}