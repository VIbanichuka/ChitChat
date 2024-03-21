using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Application.Interfaces.IRepositories;
using ChitChat.Core.Entities;
using ChitChat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChitChat.Infrastructure.Repositories
{
    public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
    {
        private readonly ChitChatDbContext _context;
        public UserProfileRepository(ChitChatDbContext context) : base(context)
        {      
            _context = context;
        }

        public async Task<IEnumerable<UserProfile>> SearchUserAsync(string user) 
        {
            return await _context.UserProfiles
                .Where(u => (u.FirstName != null && u.FirstName.ToLower().Contains(user.Trim().ToLower())) || (u.LastName != null && u.LastName.ToLower().Contains(user.Trim().ToLower())))
                .ToListAsync();
        }
    }
}
