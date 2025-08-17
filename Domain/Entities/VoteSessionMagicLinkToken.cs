using System;

namespace Domain.Entities
{
    public class VoteSessionMagicLinkToken : Entity
    {
        public int AttendanceUserId { get; set; }
        public int VoteSessionId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public bool IsUsed { get; set; }

        public AttendanceUser AttendanceUser { get; set; }
        public VoteSession VoteSession { get; set; }
    }
}
