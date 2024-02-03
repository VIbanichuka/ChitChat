using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChitChat.Infrastructure.Data
{
    public class ChitChatDbContext : DbContext
    {
        public ChitChatDbContext(DbContextOptions<ChitChatDbContext> options) : base(options)
        {
        }

        public ChitChatDbContext()
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Channel> Channels { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
    }
}
