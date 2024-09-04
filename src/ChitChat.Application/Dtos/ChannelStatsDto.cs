using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Application.Dtos
{
    public class ChannelStatsDto
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public int UserCount { get; set; }
    }
}
