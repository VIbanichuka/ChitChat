using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Application.Interfaces.IRepositories;
using ChitChat.Core.Entities;
using ChitChat.Infrastructure.Data;

namespace ChitChat.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ChitChatDbContext context) : base(context)
        {           
        }
    }
}
