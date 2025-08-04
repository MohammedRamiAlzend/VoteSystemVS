using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Vote : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the ID of the voting item this vote is for.
        /// </summary>
        public Guid VotingItemId { get; set; }
        /// <summary>
        /// Gets or sets the ID of the user who cast the vote.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Gets or sets the ID of the selected voting option, if applicable.
        /// </summary>
        public Guid? OptionId { get; set; }
        /// <summary>
        /// Gets or sets the write-in value for the vote, if applicable.
        /// </summary>
        public string WriteInValue { get; set; }
        // public DateTime VoteTimestamp { get; set; } = DateTime.Now; // To be removed, as AuditableEntity provides CreatedAtUtc
        /// <summary>
        /// Gets or sets the verification code associated with the vote.
        /// </summary>
        public string VerificationCode { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the vote has been verified.
        /// </summary>
        public bool IsVerified { get; set; } = false;
        /// <summary>
        /// Gets or sets information about the device used to cast the vote.
        /// </summary>
        public string DeviceInfo { get; set; }
        /// <summary>
        /// Gets or sets the IP address from which the vote was cast.
        /// </summary>
        public string IpAddress { get; set; }

        // العلاقات
        /// <summary>
        /// Gets or sets the voting item associated with this vote.
        /// </summary>
        public VotingItem VotingItem { get; set; }
        /// <summary>
        /// Gets or sets the user who cast this vote.
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Gets or sets the voting option selected for this vote.
        /// </summary>
        public VotingOption VotingOption { get; set; }
    }
}
