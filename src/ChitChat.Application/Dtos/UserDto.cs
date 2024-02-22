using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Core.Entities;

namespace ChitChat.Application.Dtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public List<Message>? SentMessages { get; set; }
        public List<Message>? ReceivedMessages { get; set; }
        public List<Channel>? Channels { get; set; }
    }
}
