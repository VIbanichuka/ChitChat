using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Application.Interfaces.IServices
{
    public interface IPasswordService
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] storedPasswordHash, byte[] storedPasswordSalt);
    }
}
