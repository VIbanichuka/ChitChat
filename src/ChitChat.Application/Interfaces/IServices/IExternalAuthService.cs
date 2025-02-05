using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Application.Interfaces.IServices
{
    public interface IExternalAuthService
    {
        Task<string> AuthenticateWithGoogleAsync(string? token);
    }
}
