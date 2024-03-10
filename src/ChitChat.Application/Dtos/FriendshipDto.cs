using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Core.Enums;

namespace ChitChat.Application.Dtos
{
    public class FriendshipDto
    {
        public int FriendshipId { get; set; }
        public Guid InviterId { get; set; }
        public Guid InviteeId { get; set; }
        public DateTime? InviteTime { get; set; } = DateTime.UtcNow;
        public FriendshipStatus FriendshipStatus { get; set; }
    }
}
