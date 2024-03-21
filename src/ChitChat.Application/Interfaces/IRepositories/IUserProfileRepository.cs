﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Core.Entities;

namespace ChitChat.Application.Interfaces.IRepositories
{
    public interface IUserProfileRepository : IGenericRepository<UserProfile>
    {
        Task<IEnumerable<UserProfile>> SearchUserAsync(string user);
    }
}
