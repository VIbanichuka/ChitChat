﻿using System;
using System.Collections.Generic;
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
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public List<Message>? SentMessages { get; set; }
        public List<Message>? ReceivedMessages { get; set; }
        public List<Channel>? Channels { get; set; }
        public List<Friendship> SentFriendRequests { get; set; } = null!;
        public List<Friendship> ReceivedFriendRequests { get; set; } = null!;
    }
}
