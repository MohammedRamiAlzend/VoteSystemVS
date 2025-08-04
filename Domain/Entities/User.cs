using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : AuditableEntity
    {
        // معلومات الحساب
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the hashed password of the user.
        /// </summary>
        public string PasswordHash { get; set; }
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Gets or sets the membership ID of the user in the association.
        /// </summary>
        public string MemberId { get; set; } // رقم العضوية في الجمعية
        /// <summary>
        /// Gets or sets the ID of the association this user belongs to.
        /// </summary>
        public Guid AssociationId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the user account is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        // معلومات الأمان
        /// <summary>
        /// Gets or sets the security question for password recovery.
        /// </summary>
        public string SecurityQuestion { get; set; }
        /// <summary>
        /// Gets or sets the hashed answer to the security question.
        /// </summary>
        public string SecurityAnswerHash { get; set; }

        // تواريخ مهمة
        /// <summary>
        /// Gets or sets the timestamp of the user's last login.
        /// </summary>
        public DateTime? LastLogin { get; set; }

        // العلاقات
        /// <summary>
        /// Gets or sets the collection of roles assigned to this user.
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        /// <summary>
        /// Gets or sets the collection of voting sessions created by this user.
        /// </summary>
        public ICollection<VotingSession> CreatedVotingSessions { get; set; } = new List<VotingSession>();
        /// <summary>
        /// Gets or sets the collection of votes cast by this user.
        /// </summary>
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
        /// <summary>
        /// Gets or sets the collection of verification codes associated with this user.
        /// </summary>
        public ICollection<VerificationCode> VerificationCodes { get; set; } = new List<VerificationCode>();
        /// <summary>
        /// Gets or sets the collection of audit logs related to this user.
        /// </summary>
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
        /// <summary>
        /// Gets or sets the collection of voting sessions modified by this user.
        /// </summary>
        public ICollection<VotingSession> ModifiedVotingSessions { get; set; } = new List<VotingSession>();
        /// <summary>
        /// Gets or sets the collection of system settings modified by this user.
        /// </summary>
        public ICollection<SystemSetting> ModifiedSettings { get; set; } = new List<SystemSetting>();

        // لإنشاء علاقة many-to-many ضمنية مع Role
        /// <summary>
        /// Gets or sets the collection of roles associated with this user (implicit many-to-many).
        /// </summary>
        public ICollection<Role> Roles { get; set; } = new List<Role>();
        /// <summary>
        /// Gets or sets the association this user belongs to.
        /// </summary>
        public Association Association { get; set; }
    }
}
