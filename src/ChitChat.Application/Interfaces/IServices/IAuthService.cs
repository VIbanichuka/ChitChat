using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Application.Dtos;
using ChitChat.Core.Entities;

namespace ChitChat.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        string CreateToken(User user);
    }
}
