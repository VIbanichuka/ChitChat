using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Core.Entities
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(30)]
        [Display(Name = "Display Name")]
        public string? DisplayName { get; set; }

        [Required]
        [MaxLength(300)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; } = new byte[32];

        [Required]
        public byte[] PasswordSalt { get; set; } = new byte[32];

        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<Message> SentMessages { get; set; } = null!;

        public virtual ICollection<Message> ReceivedMessages { get; set; } = null!;
        public virtual ICollection<Friendship> SentFriendRequests { get; set; } = null!;
        public virtual ICollection<Friendship> ReceivedFriendRequests { get; set; } = null!;
        public virtual ICollection<Channel> Channels { get; set; } = null!;
        public virtual ICollection<UserConnection> Connections { get; set; } = null!;
    }
}
