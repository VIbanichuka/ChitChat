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
    public class ChannelRepository : GenericRepository<Channel>, IChannelRepository
    {
        private readonly ChitChatDbContext _context;
        public ChannelRepository(ChitChatDbContext context) : base(context)
        {          
            _context = context;
        }

        public async Task<IEnumerable<Channel>> GetChannelsByUserIdAsync(Guid userId)
        {
            return await _context.Channels.Include(c => c.Users)
                .Where(c => c.Users.Any(u => u.UserId == userId))
                .ToListAsync();
        }

        public async Task<Channel?> GetChannelWithUserByIdAsync(int channelId) 
        {
            return await _context.Channels.Include(c => c.Users).FirstOrDefaultAsync(c => c.ChannelId == channelId);
        }
    }
}
