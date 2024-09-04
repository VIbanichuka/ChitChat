using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Application.Dtos
{
    public class FriendshipRequestStatsDto
    {
        public int Count { get; set; }
        public string FriendshipStatus { get; set; }
        public DateTime InviteTime { get; set; }
    }
}
