﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Application.Dtos
{
    public class FriendDto
    {
        public Guid UserId { get; set; }
        public string? DisplayName { get; set; }

        public FriendDto(Guid userId, string displayName)
        {
            UserId = userId;
            DisplayName = displayName;
        }
    } 
}
