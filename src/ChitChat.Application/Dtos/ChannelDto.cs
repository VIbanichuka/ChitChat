using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Application.Dtos
{
    public class ChannelDto
    {
        public int ChannelId { get; set; }

        public string? ChannelName { get; set; }
        
        List<UserDto>? Users { get; set; }
    }
}
