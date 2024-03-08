using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Core.Enums;

namespace ChitChat.Core.Entities
{
    public class Friendship
    {
        [Key]
        public int FriendshipId { get; set; }
        public Guid InviterId { get; set; }
        public Guid InviteeId { get; set; }
        public DateTime? InviteTime { get; set; } = DateTime.UtcNow;
        public FriendShipStatus FriendShipStatus { get; set; }

        [ForeignKey(nameof(InviterId))]
        public virtual User? Inviter { get; set; }

        [ForeignKey(nameof(InviteeId))]
        public virtual User? Invitee { get; set; }
    }
}
